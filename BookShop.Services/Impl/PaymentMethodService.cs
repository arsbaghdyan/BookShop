using AutoMapper;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BookShop.Services.Impl;

internal class PaymentMethodService : IPaymentMethodService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<PaymentMethodService> _logger;
    private readonly ICustomAuthenticationService _customAuthenticationService;
    private readonly IMapper _mapper;

    public PaymentMethodService(BookShopDbContext bookShopDbContext,
        ILogger<PaymentMethodService> logger, ICustomAuthenticationService customAuthenticationService, IMapper mapper)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _customAuthenticationService = customAuthenticationService;
        _mapper = mapper;
    }

    public async Task AddAsync(PaymentMethodAddVm paymentMethod)
    {
        if (paymentMethod == null)
        {
            throw new Exception("There is nothing to add");
        }

        var client = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == paymentMethod.ClientId);

        if (client == null)
        {
            throw new Exception("Client not Found");
        }

        var checkingClientEmail = _customAuthenticationService.GetClientEmailFromToken();

        if (client.Email != checkingClientEmail)
        {
            throw new Exception("Unauthorized: You can only add your own paymentMethod.");
        }

        paymentMethod.Details = SerializeDetails(paymentMethod.Details);

        var paymentMethodToAdd = _mapper.Map<PaymentMethodEntity>(paymentMethod);

        _bookShopDbContext.PaymentMethods.Add(paymentMethodToAdd);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"PaymentMethod with Id {paymentMethodToAdd.Id} added successfully.");
    }

    public async Task<List<PaymentMethodGetVm>> GetAllAsync(long clientId)
    {
        var client = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);

        if (client == null)
        {
            throw new Exception("Client not Found");
        }

        var paymentMethods = await _bookShopDbContext.PaymentMethods.Where(pm => pm.ClientId == clientId).ToListAsync();

        return _mapper.Map<List<PaymentMethodGetVm>>(paymentMethods);
    }

    public async Task RemoveAsync(long paymentMethodId)
    {
        var paymentMethod = await _bookShopDbContext.PaymentMethods.FirstOrDefaultAsync(p => p.Id == paymentMethodId);

        if (paymentMethod == null)
        {
            throw new Exception("PaymentMethod not found");
        }

        var client = await _bookShopDbContext.Clients.FirstOrDefaultAsync(p => p.Id == paymentMethod.ClientId);

        if (client == null)
        {
            throw new Exception("There is no matching Client");
        }

        var checkingClientEmail = _customAuthenticationService.GetClientEmailFromToken();

        if (client.Email != checkingClientEmail)
        {
            throw new InvalidOperationException("Unauthorized: You can only remove your own paymentMethod.");
        }

        _bookShopDbContext.PaymentMethods.Remove(paymentMethod);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"PaymentMethod with Id {paymentMethod.Id} removed successfully.");
    }

    private string SerializeDetails(string details)
    {
        return JsonConvert.SerializeObject(details);
    }
}
