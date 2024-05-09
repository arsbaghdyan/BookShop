using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

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
            clientEntity.Password = HashPassword(clientEntity.Password);

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
            var clientToRemove = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Email == clientEntity.Email);

            if (clientToRemove == null)
            {
                throw new Exception("Client not found");
            }

            clientEntity.Password = HashPassword(clientEntity.Password);

            if (clientEntity.Password != clientToRemove.Password)
            {
                throw new Exception("Invalid password");
            }

            _bookShopDbContext.Clients.Remove(clientToRemove);
            await _bookShopDbContext.SaveChangesAsync();
            _logger.LogInformation($"Client with Id {clientToRemove.Id} removed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while retrieving client with Id {clientToRemove.Id}.");
            throw;
        }
    }

    public async Task UpdateAsync(ClientEntity clientEntity)
    {
        try
        {
            var clientToUpdate = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Email == clientEntity.Email);

            if (clientToUpdate == null)
            {
                throw new Exception("Client with current email not found");
            }

            clientToUpdate.FirstName = clientEntity.FirstName;
            clientToUpdate.LastName = clientEntity.LastName;
            clientToUpdate.Email = clientEntity.Email;
            clientToUpdate.Address = clientEntity.Address;
            clientToUpdate.Password = clientEntity.Password;

            if (!string.IsNullOrEmpty(clientEntity.Password))
            {
                clientToUpdate.Password = HashPassword(clientEntity.Password);
            }

            await _bookShopDbContext.SaveChangesAsync();
            _logger.LogInformation($"Client with Id {clientEntity.Id} updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while updating client with Id {clientEntity.Id}.");
            throw;
        }
    }
    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
            {
                builder.Append(hashedBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
