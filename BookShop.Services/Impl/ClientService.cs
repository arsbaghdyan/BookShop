using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
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
    private readonly IClientContextReader _clientContextReader;

    public ClientService(BookShopDbContext bookShopDbContext,
                         ILogger<ClientService> logger,
                         IMapper mapper,
                         IClientContextReader clientContextReader)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _mapper = mapper;
        _clientContextReader = clientContextReader;
    }

    public async Task<ClientModel?> GetClientAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();
        var client = await _bookShopDbContext.Clients
            .FirstOrDefaultAsync(p => p.Id == clientId);

        return _mapper.Map<ClientModel?>(client);
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

    public async Task<ClientModel?> RegisterAsync(ClientRegisterModel clientRegisterModel)
    {
        var clientToAdd = _mapper.Map<ClientEntity>(clientRegisterModel);
        clientToAdd.Password = HashPassword(clientRegisterModel.Password);

        _bookShopDbContext.Clients.Add(clientToAdd);

        var newCart = new CartEntity { Client = clientToAdd };
        _bookShopDbContext.Carts.Add(newCart);

        var newWishList = new WishListEntity { Client = clientToAdd };
        _bookShopDbContext.WishLists.Add(newWishList);

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Client with {clientToAdd.Id} Id added successfully.");

        return _mapper.Map<ClientModel?>(clientToAdd);
    }

    public async Task<ClientModel?> UpdateAsync(ClientUpdateModel clientUpdateModel)
    {
        var clientId = _clientContextReader.GetClientContextId();
        var clientToUpdate = await _bookShopDbContext.Clients
            .FirstOrDefaultAsync(c => c.Id == clientId);

        if (clientToUpdate==null)
        {
            throw new Exception("Client not found");
        }

        clientToUpdate.FirstName = clientUpdateModel.FirstName;
        clientToUpdate.LastName = clientUpdateModel.LastName;
        clientToUpdate.Email = clientUpdateModel.Email;
        clientToUpdate.Address = clientUpdateModel.Address;

        if (!string.IsNullOrEmpty(clientUpdateModel.Password))
        {
            clientToUpdate.Password = HashPassword(clientUpdateModel.Password);
        }

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Client with {clientId} Id modified successfully.");

        return _mapper.Map<ClientModel?>(clientToUpdate);
    }

    public async Task RemoveAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();

        await _bookShopDbContext.Clients
            .Where(c => c.Id == clientId)
            .ExecuteDeleteAsync();

        _logger.LogInformation($"Client with {clientId} Id removed successfully.");
    }

    private string HashPassword(string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var passwordHash = SHA256.HashData(passwordBytes);

        return Convert.ToHexString(passwordHash);
    }
}