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
    private readonly ICustomAuthenticationService _customAuthenticationService;

    public ClientService(BookShopDbContext bookShopDbContext, ILogger<ClientService> logger,
        ICustomAuthenticationService customAuthenticationService)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _customAuthenticationService = customAuthenticationService;
    }

    public async Task RegisterAsync(ClientEntity clientEntity)
    {
        clientEntity.Password = HashPassword(clientEntity.Password);

        _bookShopDbContext.Clients.Add(clientEntity);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Client with Id {clientEntity.Id} added successfully.");
    }

    public async Task UpdateAsync(ClientEntity clientEntity)
    {
        var checkingClientEmail = _customAuthenticationService.GetClientEmailFromToken();

        var clientToUpdate = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientEntity.Id);

        if (clientToUpdate is null)
        {
            throw new InvalidOperationException("Client not found");
        }

        if (clientToUpdate.Email != checkingClientEmail)
        {
            throw new InvalidOperationException("Unauthorized: You can only update your own client information.");
        }

        clientToUpdate.FirstName = clientEntity.FirstName;
        clientToUpdate.LastName = clientEntity.LastName;
        clientToUpdate.Email = clientEntity.Email;
        clientToUpdate.Address = clientEntity.Address;

        if (!string.IsNullOrEmpty(clientEntity.Password))
        {
            clientToUpdate.Password = HashPassword(clientEntity.Password);
        }

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Client with Id {clientEntity.Id} modified successfully.");
    }

    public async Task RemoveAsync(ClientEntity clientEntity)
    {
        var clientToRemove = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientEntity.Id);

        if (clientToRemove is null)
        {
            throw new Exception("There is no matching Client");
        }

        var checkingClientEmail = _customAuthenticationService.GetClientEmailFromToken();

        if (clientToRemove.Email != checkingClientEmail)
        {
            throw new InvalidOperationException("Unauthorized: You can only remove your own client.");
        }

        _bookShopDbContext.Clients.Remove(clientToRemove);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Client with Id {clientToRemove.Id} removed successfully.");
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

    public async Task<ClientEntity> GetByIdAsync(long clientId)
    {
        var client = await _bookShopDbContext.Clients.FirstOrDefaultAsync(p => p.Id == clientId);

        if (client == null)
        {
            throw new Exception("Client not found");
        }

        return client;
    }
}