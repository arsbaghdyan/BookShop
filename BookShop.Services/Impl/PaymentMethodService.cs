using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BookShop.Services.Impl;

internal class PaymentMethodService : IPaymentMethodService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<PaymentMethodService> _logger;
    private readonly ICustomAuthenticationService _customAuthenticationService;

    public PaymentMethodService(BookShopDbContext bookShopDbContext,
        ILogger<PaymentMethodService> logger, ICustomAuthenticationService customAuthenticationService = null)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _customAuthenticationService = customAuthenticationService;
    }

    public async Task AddAsync(PaymentMethodEntity paymentMethodEntity)
    {
        try
        {
            if (paymentMethodEntity == null)
            {
                throw new Exception("There is nothing to add");
            }

            var client = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == paymentMethodEntity.ClientId);

            if (client == null)
            {
                throw new Exception("Client not Found");
            }

            var checkingClientEmail = _customAuthenticationService.GetClientEmailFromToken();

            if (client.Email != checkingClientEmail)
            {
                throw new Exception("Unauthorized: You can only add your own paymentMethod.");
            }

            paymentMethodEntity.Details = SerializeDetails(paymentMethodEntity.Details);

            _bookShopDbContext.PaymentMethods.Add(paymentMethodEntity);
            await _bookShopDbContext.SaveChangesAsync();
            _logger.LogInformation($"PaymentMethod with Id {paymentMethodEntity.Id} added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error {ex.Message}");
            throw;
        }
    }

    public async Task<List<PaymentMethodEntity>> GetAllAsync(long clientId)
    {
        try
        {
            var client = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);

            if (client == null)
            {
                throw new Exception("Client not Found");
            }

            var checkingClientEmail = _customAuthenticationService.GetClientEmailFromToken();

            if (client.Email != checkingClientEmail)
            {
                throw new Exception("Unauthorized: You can only add your own paymentMethod.");
            }

            return await _bookShopDbContext.PaymentMethods.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error {ex.Message}");
            throw;
        }
    }

    public async Task RemoveAsync(PaymentMethodEntity paymentMethodEntity)
    {
        try
        {
            var paymentMethod = await _bookShopDbContext.PaymentMethods.FirstOrDefaultAsync(p => p.Id == paymentMethodEntity.Id);

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
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(PaymentMethodEntity paymentMethodEntity)
    {
        var paymentMethod = await _bookShopDbContext.PaymentMethods.FirstOrDefaultAsync(c => c.Id == paymentMethodEntity.Id);

        if (paymentMethod == null)
        {
            throw new Exception("PaymentMethod not found");
        }

        var client = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == paymentMethod.Id);

        if (client == null)
        {
            throw new Exception("PaymentMethod not Found");
        }

        var checkingClientEmail = _customAuthenticationService.GetClientEmailFromToken();

        if (client.Email != checkingClientEmail)
        {
            throw new Exception("Unauthorized: You can not update other client paymentMethod.");
        }

        var paymentMethodToUpdate =await _bookShopDbContext.PaymentMethods.FirstOrDefaultAsync(c => c.Id == paymentMethodEntity.Id);

        if (paymentMethodToUpdate == null)
        {
            throw new Exception("PaymentMethod not found");
        }

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"CartItem with Id {paymentMethodEntity.Id} removed from Cart with Id {paymentMethod.Id} ");
    }

    private string SerializeDetails(string details)
    {
        return JsonConvert.SerializeObject(details);
    }
}
