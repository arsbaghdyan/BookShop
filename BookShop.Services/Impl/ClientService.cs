using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BookShop.Services.Impl;

internal class ClientService : IClientService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<ClientService> _logger;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly JwtOptions _jwtOptions;

    public ClientService(BookShopDbContext bookShopDbContext, ILogger<ClientService> logger,
        IHttpContextAccessor contextAccessor, JwtOptions jwtOptions)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _contextAccessor = contextAccessor;
        _jwtOptions = jwtOptions;
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
            _logger.LogError(ex, $"Error {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(ClientEntity clientEntity)
    {
        try
        {
            var checkingClientEmail = GetClientEmailFromToken();

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
            _logger.Log(LogLevel.Information, $"Client with Id {clientEntity.Id} modified successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            throw;
        }
    }

    public async Task RemoveAsync(long clientId)
    {
        try
        {
            var clientToRemove = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);

            if (clientToRemove is null)
            {
                throw new Exception("There is no matching Client");
            }

            var checkingClientEmail = GetClientEmailFromToken();

            if (clientToRemove.Email != checkingClientEmail)
            {
                throw new InvalidOperationException("Unauthorized: You can only remove your own client.");
            }

            _bookShopDbContext.Clients.Remove(clientToRemove);
            await _bookShopDbContext.SaveChangesAsync();
            _logger.Log(LogLevel.Information, $"Client with Id {clientToRemove.Id} removed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            throw;
        }
    }

    private string GetClientEmailFromToken()
    {
        var token = _contextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Key);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var clientEmail = jwtToken.Claims.First(x => x.Type == ClaimTypes.Email).Value;

            return clientEmail;
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

    public async Task<ClientEntity> GetByIdAsync(long clientId)
    {
        try
        {
            var client = await _bookShopDbContext.Clients.FirstOrDefaultAsync(p => p.Id == clientId);

            if (client == null)
            {
                throw new Exception("Client not found");
            }

            return client;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error: {ex.Message}");
            throw;
        }
    }
}