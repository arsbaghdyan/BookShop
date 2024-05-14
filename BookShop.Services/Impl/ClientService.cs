using AutoMapper;
using BookShop.Common.ClientService;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using BookShop.Services.Models.ClientModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace BookShop.Services.Impl;

internal class ClientService : IClientService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<ClientService> _logger;
    private readonly IMapper _mapper;
    private readonly ClientContextReader _clientContextReader;

    public ClientService(BookShopDbContext bookShopDbContext, ILogger<ClientService> logger,
                         IMapper mapper, ClientContextReader clientContextReader)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _mapper = mapper;
        _clientContextReader = clientContextReader;
    }

    public async Task<ClientModel> RegisterAsync(ClientRegisterModel clientRegisterModel)
    {
        using (var transaction = _bookShopDbContext.Database.BeginTransaction())
        {
            try
            {
                var clientToAdd = _mapper.Map<ClientEntity>(clientRegisterModel);
                clientToAdd.Password = HashPassword(clientRegisterModel.Password);

                _bookShopDbContext.Clients.Add(clientToAdd);
                await _bookShopDbContext.SaveChangesAsync();

                var newCart = new CartEntity { ClientId = clientToAdd.Id };
                _bookShopDbContext.Carts.Add(newCart);

                var newWishlist = new WishListEntity { ClientId = clientToAdd.Id };
                _bookShopDbContext.WishLists.Add(newWishlist);

                await _bookShopDbContext.SaveChangesAsync();
                _logger.LogInformation($"Client with Id {clientToAdd.Id} added successfully.");

                var clientModel = _mapper.Map<ClientModel>(clientToAdd);

                transaction.Commit();
                return clientModel;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public async Task<ClientModel> UpdateAsync(ClientUpdateModel client)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var clientToUpdate = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);

        clientToUpdate.FirstName = client.FirstName;
        clientToUpdate.LastName = client.LastName;
        clientToUpdate.Email = client.Email;
        clientToUpdate.Address = client.Address;

        if (!string.IsNullOrEmpty(client.Password))
        {
            clientToUpdate.Password = HashPassword(client.Password);
        }

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Client with Id {clientId} modified successfully.");

        var clientModel = _mapper.Map<ClientModel>(clientToUpdate);

        return clientModel;
    }

    public async Task RemoveAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();

        var clientToRemove = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);

        _bookShopDbContext.Clients.Remove(clientToRemove);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Client with Id {clientToRemove.Id} removed successfully.");
    }

    public async Task<ClientModel?> GetClientAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();

        var client = await _bookShopDbContext.Clients.FirstOrDefaultAsync(p => p.Id == clientId);

        var getClient = _mapper.Map<ClientModel?>(client);

        return getClient;
    }

    public async Task<ClientModel?> GetByEmailAndPasswordAsync(
        string email,
        string password)
    {
        var client = await _bookShopDbContext.Clients
            .FirstOrDefaultAsync(p => p.Email == email);

        if (client != null)
        {
            var hashedPassword = HashPassword(password);
            if (client.Password == hashedPassword)
            {
                return _mapper.Map<ClientModel?>(client);
            }
        }

        return null;
    }

    private string HashPassword(string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var passwordHash = SHA256.HashData(passwordBytes);
        return Convert.ToHexString(passwordHash);
    }
}