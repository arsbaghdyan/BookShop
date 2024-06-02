using BookShop.Common.ClientService.Abstractions;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.InvoiceModels;

namespace BookShop.Api.Services;

public class InvoicePaymentJob : BackgroundService
{
    private const int PeriodInMin = 60;
    private readonly TimeSpan _period = TimeSpan.FromMinutes(PeriodInMin);

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<InvoicePaymentJob> _logger;

    public InvoicePaymentJob(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<InvoicePaymentJob> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_period);

        while (!stoppingToken.IsCancellationRequested &&
            await timer.WaitForNextTickAsync(stoppingToken))
        {
            using var serviceScope = _serviceScopeFactory.CreateScope();

            var invoiceService = serviceScope.ServiceProvider.GetRequiredService<IInvoiceService>();
            var paymentService = serviceScope.ServiceProvider.GetRequiredService<IPaymentService>();
            var contextWriter = serviceScope.ServiceProvider.GetRequiredService<IClientContextWriter>();

            var declinedPayments = await invoiceService.GetDeclinedInvoicesAsync();

            Dictionary<long, List<InvoiceModel>> invoicesByClientId =
                declinedPayments.GroupBy(i => i.ClientId).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var pair in invoicesByClientId)
            {
                contextWriter.SetClientContextId(pair.Key);

                foreach (var invoice in pair.Value)
                {
                    await paymentService.PayAsync(invoice.Id);
                }
            }
        }
    }
}
