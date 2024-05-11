using AutoMapper;
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
    private readonly ICartService _cartService;
    private readonly IWishListService _wishListService;

    public ClientService(BookShopDbContext bookShopDbContext, ILogger<ClientService> logger,
                         IMapper mapper, ICartService cartService, IWishListService wishListService)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _mapper = mapper;
        _cartService = cartService;
        _wishListService = wishListService;
    }

    public async Task RegisterAsync(ClientRegisterVm client)
    {
        client.Password = HashPassword(client.Password);

        var clientToAdd = _mapper.Map<ClientEntity>(client);

        _bookShopDbContext.Clients.Add(clientToAdd);
        await _bookShopDbContext.SaveChangesAsync();

        await _wishListService.CreateAsync(clientToAdd.Id);
        await _cartService.CreateAsync(clientToAdd.Id);

        _logger.LogInformation($"Client with Id {clientToAdd.Id} added successfully.");
    }

    public async Task UpdateAsync(ClientUpdateVm client)
    {

        var clientToUpdate = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == client.Id);

        if (clientToUpdate is null)
        {
            throw new InvalidOperationException("Client not found");
        }

        clientToUpdate.FirstName = client.FirstName;
        clientToUpdate.LastName = client.LastName;
        clientToUpdate.Email = client.Email;
        clientToUpdate.Address = client.Address;

        if (!string.IsNullOrEmpty(client.Password))
        {
            clientToUpdate.Password = HashPassword(client.Password);
        }

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Client with Id {client.Id} modified successfully.");
    }

    public async Task RemoveAsync(long clientId)
    {
        var clientToRemove = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);

        if (clientToRemove is null)
        {
            throw new Exception("There is no matching Client");
        }

        _bookShopDbContext.Clients.Remove(clientToRemove);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Client with Id {clientToRemove.Id} removed successfully.");
    }

    private string HashPassword(string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var passwordHash = SHA256.HashData(passwordBytes);
        return Convert.ToHexString(passwordHash);
    }

    public async Task<ClientGetVm> GetByIdAsync(long clientId)
    {
        var client = await _bookShopDbContext.Clients.FirstOrDefaultAsync(p => p.Id == clientId);

        if (client == null)
        {
            throw new Exception("Client not found");
        }

        var getClient = _mapper.Map<ClientGetVm>(client);

        return getClient;
    }
}