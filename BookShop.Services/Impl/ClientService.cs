using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookShop.Services.Impl;

internal class ClientService : IClientService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<ClientService> _logger;

    public ClientService(BookShopDbContext bookShopDbContext, ILogger<ClientService> logger)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
    }

    public async Task RegisterAsync(ClientEntity clientEntity)
    {
        try
        {
            _bookShopDbContext.Clients.Add(clientEntity);
            await _bookShopDbContext.SaveChangesAsync();
            _logger.LogInformation($"Client with Id {clientEntity.Id} added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while adding client.");
        }
    }

    public async Task RemoveAsync(ClientEntity clientEntity)
    {
        try
        {
            var clientToRemove = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientEntity.Id);

            if (clientToRemove == null)
            {
                throw new Exception("Client not found");
            }
            _bookShopDbContext.Clients.Remove(clientToRemove);
            await _bookShopDbContext.SaveChangesAsync();
            _logger.LogInformation($"Client with Id {clientEntity.Id} removed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while retrieving client with Id {clientEntity.Id}.");
            throw;
        }
    }

    public async Task UpdateAsync(ClientEntity clientEntity)
    {
        try
        {
            var clientToUpdate = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientEntity.Id);
            if (clientToUpdate == null)
            {
                throw new Exception("Client not found");
            }

            clientToUpdate.FirstName = clientEntity.FirstName;
            clientToUpdate.LastName = clientEntity.LastName;
            clientToUpdate.Email = clientEntity.Email;
            clientToUpdate.Address = clientEntity.Address;
            clientToUpdate.Password = clientEntity.Password;
            await _bookShopDbContext.SaveChangesAsync();
            _logger.LogInformation($"Client with Id {clientEntity.Id} updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while updating client with Id {clientEntity.Id}.");
            throw;
        }
    }
}
