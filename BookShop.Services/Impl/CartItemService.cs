using BookShop.Data.Entities;
using BookShop.Data;
using BookShop.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BookShop.Services.Models.CartItemModels;
using AutoMapper;
using BookShop.Common.ClientService.Abstractions;

namespace BookShop.Services.Impl;

internal class CartItemService : ICartItemService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<CartItemService> _logger;
    private readonly IMapper _mapper;
    private readonly IClientContextReader _clientContextReader;

    public CartItemService(BookShopDbContext bookShopDbContext, ILogger<CartItemService> logger, IMapper mapper, IClientContextReader clientContextReader)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _mapper = mapper;
        _clientContextReader = clientContextReader;
    }

    public async Task<CartItemModel> AddAsync(CartItemAddModel cartItemAddModel)
    {
        if (cartItemAddModel.Count <= 0)
        {
            throw new Exception("Produc count cant be less than 0");
        }

        var clientId = _clientContextReader.GetClientContextId();
        var cart = await _bookShopDbContext.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.ClientId == clientId);
        var product = await _bookShopDbContext.Products.FirstOrDefaultAsync(p => p.Id == cartItemAddModel.ProductId);

        if (product.Count < cartItemAddModel.Count)
        {
            throw new Exception("Not enough product");
        }

        var cartItemCheck = cart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItemAddModel.ProductId
                                                                              && ci.CartId == cart.Id);
        var cartItemModel = new CartItemModel();
        if (cartItemCheck != null)
        {
            cartItemCheck.Count += cartItemAddModel.Count;
            cartItemCheck.Price = cartItemCheck.Count * product.Price;
            await _bookShopDbContext.SaveChangesAsync();
            cartItemModel = _mapper.Map<CartItemModel>(cartItemCheck);

            return cartItemModel;
        }
        var cartItem = _mapper.Map<CartItemEntity>(cartItemAddModel);

        cartItem.Price = cartItem.Count * product.Price;
        cartItem.CartId = cart.Id;

        _bookShopDbContext.CartItems.Add(cartItem);

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"CartItem with Id {cartItem.Id} added successfully for client with id {clientId}.");

        cartItemModel = _mapper.Map<CartItemModel>(cartItem);

        return cartItemModel;
    }

    public async Task RemoveAsync(long cartItemId)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var cart = await _bookShopDbContext.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.ClientId == clientId);

        var cartEntity = cart.CartItems.FirstOrDefault(c => c.Id == cartItemId);

        _bookShopDbContext.CartItems.Remove(cartEntity);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Cart with Id {cartEntity.Id} remove succesfully for client with id {clientId}.");
    }

    public async Task<CartItemModel> UpdateAsync(CartItemUpdateModel cartItemUpdateModel)
    {
        if (cartItemUpdateModel.Count <= 0)
        {
            throw new Exception("Produc count cant be less than 0");
        }

        var clientId = _clientContextReader.GetClientContextId();

        var cart = await _bookShopDbContext.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.ClientId == clientId);
        var product = await _bookShopDbContext.Products.FirstOrDefaultAsync(p => p.Id == cartItemUpdateModel.ProductId);

        if (product.Count < cartItemUpdateModel.Count)
        {
            throw new Exception("Not enough product");
        }

        var cartEntity = cart.CartItems.FirstOrDefault(c => c.Id == cartItemUpdateModel.Id);

        cartEntity.Count = cartItemUpdateModel.Count;
        cartEntity.Price = product.Price * cartEntity.Count;

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"CartItem with Id {cartItemUpdateModel.Id} removed successfully for client with id {clientId}.");

        var cartItemModel = _mapper.Map<CartItemModel>(cartEntity);

        return cartItemModel;
    }
}