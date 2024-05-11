﻿using AutoMapper;
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
    private readonly ICustomAuthenticationService _customAuthenticationService;
    private readonly IMapper _mapper;
    private readonly ICartService _cartService;
    private readonly IWishListService _wishListService;

    public ClientService(BookShopDbContext bookShopDbContext, ILogger<ClientService> logger,
        ICustomAuthenticationService customAuthenticationService, IMapper mapper, ICartService cartService, IWishListService wishListService)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _customAuthenticationService = customAuthenticationService;
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
        var checkingClientEmail = _customAuthenticationService.GetClientEmailFromToken();

        var clientToUpdate = await _bookShopDbContext.Clients.FirstOrDefaultAsync(c => c.Id == client.Id);

        if (clientToUpdate is null)
        {
            throw new InvalidOperationException("Client not found");
        }

        if (clientToUpdate.Email != checkingClientEmail)
        {
            throw new InvalidOperationException("Unauthorized: You can only update your own client information.");
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