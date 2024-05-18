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

    public CartItemService(BookShopDbContext bookShopDbContext, ILogger<CartItemService> logger,
                           IMapper mapper, IClientContextReader clientContextReader)
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
            throw new Exception("Product count cant be less than 0");
        }

        var clientId = _clientContextReader.GetClientContextId();
        var cartEntity = await _bookShopDbContext.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.ClientId == clientId);
        var productEntity = await _bookShopDbContext.Products.FirstOrDefaultAsync(p => p.Id == cartItemAddModel.ProductId);

        if (productEntity == null)
        {
            throw new Exception($"Input parametr productId {cartItemAddModel.ProductId} is invalid");
        }

        if (productEntity.Count < cartItemAddModel.Count)
        {
            throw new Exception("Not enough product");
        }

        var cartItemEntity = cartEntity.CartItems.FirstOrDefault(ci => ci.ProductId == cartItemAddModel.ProductId
                                                                              && ci.CartId == cartEntity.Id);
        var cartItemModel = new CartItemModel();
        if (cartItemEntity != null)
        {
            cartItemEntity.Count += cartItemAddModel.Count;
            cartItemEntity.Price = cartItemEntity.Count * productEntity.Price;

            await _bookShopDbContext.SaveChangesAsync();
            cartItemModel = _mapper.Map<CartItemModel>(cartItemEntity);

            return cartItemModel;
        }
        var cartItemToAdd = _mapper.Map<CartItemEntity>(cartItemAddModel);

        cartItemToAdd.Price = cartItemToAdd.Count * productEntity.Price;
        cartItemToAdd.CartId = cartEntity.Id;

        _bookShopDbContext.CartItems.Add(cartItemToAdd);

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"CartItem with Id {cartItemToAdd.Id} added successfully for client with id {clientId}.");

        cartItemModel = _mapper.Map<CartItemModel>(cartItemToAdd);

        return cartItemModel;
    }

    public async Task<CartItemModel> AddFromWishList(CartItemFromWishListModel cartItemFromWishListModel)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var cartEntity = await _bookShopDbContext.Carts.Include(c => c.CartItems)
                                                 .FirstOrDefaultAsync(c => c.ClientId == clientId);

        var wishListItem = await _bookShopDbContext.WishListItems.Include(w => w.ProductEntity)
                                                   .FirstOrDefaultAsync(w => w.Id == cartItemFromWishListModel.WishListItemId);

        if (cartEntity == null || wishListItem == null)
        {
            throw new Exception("Invalid client or wishlist item.");
        }

        var existingCartItem = cartEntity.CartItems.FirstOrDefault(ci => ci.ProductId == wishListItem.ProductId);

        if (existingCartItem != null)
        {
            existingCartItem.Count += cartItemFromWishListModel.Count;
            await _bookShopDbContext.SaveChangesAsync();

            var cartItemModel = _mapper.Map<CartItemModel>(existingCartItem);
            return cartItemModel;
        }
        else
        {
            var cartItem = new CartItemEntity
            {
                ProductId = wishListItem.ProductId,
                Count = cartItemFromWishListModel.Count,
                Price = wishListItem.ProductEntity.Price,
                CartId = cartEntity.Id
            };

            _bookShopDbContext.CartItems.Add(cartItem);
            await _bookShopDbContext.SaveChangesAsync();

            var cartItemModel = _mapper.Map<CartItemModel>(cartItem);
            return cartItemModel;
        }
    }

    public async Task RemoveAsync(long cartItemId)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var cartEntity = await _bookShopDbContext.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.ClientId == clientId);
        var cartItemEntity = cartEntity.CartItems.FirstOrDefault(c => c.Id == cartItemId);

        _bookShopDbContext.CartItems.Remove(cartItemEntity);
        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Cart with Id {cartItemEntity.Id} remove succesfully for client with id {clientId}.");
    }

    public async Task<CartItemModel> UpdateAsync(CartItemUpdateModel cartItemUpdateModel)
    {
        if (cartItemUpdateModel.Count <= 0)
        {
            throw new Exception("Product count cant be less than 0");
        }

        var clientId = _clientContextReader.GetClientContextId();

        var cartEntity = await _bookShopDbContext.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.ClientId == clientId);
        var productEntity = await _bookShopDbContext.Products.FirstOrDefaultAsync(p => p.Id == cartItemUpdateModel.ProductId);

        if (productEntity.Count < cartItemUpdateModel.Count)
        {
            throw new Exception("Not enough product");
        }

        var cartItemEntity = cartEntity.CartItems.FirstOrDefault(c => c.Id == cartItemUpdateModel.Id);

        cartItemEntity.Count = cartItemUpdateModel.Count;
        cartItemEntity.Price = productEntity.Price * cartItemEntity.Count;

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"CartItem with Id {cartItemUpdateModel.Id} removed successfully for client with id {clientId}.");

        var cartItemModel = _mapper.Map<CartItemModel>(cartItemEntity);

        return cartItemModel;
    }
}