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
    private readonly ILogger<ClientService> _loggerService;
    private readonly ICustomAuthenticationService _customAuthenticationService;

    public ClientService(BookShopDbContext bookShopDbContext,
                         ILogger<ClientService> loggerService,
                         ICustomAuthenticationService customAuthenticationService)
    {
        _bookShopDbContext = bookShopDbContext;
        _loggerService = loggerService;
        _customAuthenticationService = customAuthenticationService;
    }
    public async Task RegisterAsync(ClientEntity clientEntity)
    {
        try
        {
            clientEntity.Password = HashPassword(clientEntity.Password);
            _bookShopDbContext.Clients.Add(clientEntity);
            await _bookShopDbContext.SaveChangesAsync();
            _loggerService.LogInformation($"Client with Id {clientEntity.Id} added successfully.");
        }
        catch (Exception ex)
        {
            _loggerService.LogError(ex, $"Error occurred while adding client.");
            throw;
        }
    }
    public async Task UpdateAsync(ClientEntity clientEntity)
    {
        try
        {
            var clientId = GetClientIdFromToken(clientEntity);

            var clientToUpdate = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientEntity.Id);

            if (clientToUpdate is null)
            {
                throw new InvalidOperationException("Client not found");
            }

            if (clientToUpdate.Id != clientId)
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
            _loggerService.Log(LogLevel.Information, $"Client with Id {clientEntity.Id} modified successfully.");
        }
        catch (Exception ex)
        {
            _loggerService.LogError($"Error: {ex.Message}");
            throw;
        }
    }
    public async Task RemoveAsync(long clientId)
    {
        try
        {
            var clientToRemove = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
            var requestClientId = GetClientIdFromToken(clientToRemove);
            if (clientId != requestClientId)
            {
                throw new InvalidOperationException("Unauthorized: You can only remove your own client.");
            }
            if (clientToRemove is null)
            {
                throw new Exception("There is no matching Client");
            }
            _bookShopDbContext.Clients.Remove(clientToRemove);
            await _bookShopDbContext.SaveChangesAsync();
            _loggerService.Log(LogLevel.Information, $"Client with Id {clientToRemove.Id} removed successfully.");
        }
        catch (Exception ex)
        {
            _loggerService.LogError($"Error: {ex.Message}");
            throw;
        }
    }
    private long GetClientIdFromToken(ClientEntity clientEntity)
    {
        var token = _customAuthenticationService.GenerateToken(clientEntity);

        if (token != null)
        {
            var clientId = long.Parse(token.Claims.First(x => x.Type == "clientId").Value);

            return clientId;
        }
        throw new InvalidOperationException("Token not found.");
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
