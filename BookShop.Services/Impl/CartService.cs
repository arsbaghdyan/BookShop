using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Services.Abstractions;
using BookShop.Services.Models.CartItemModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookShop.Services.Impl;

internal class CartService : ICartService
{
    private readonly BookShopDbContext _bookShopDbContext;
    private readonly ILogger<CartService> _logger;
    private readonly IMapper _mapper;
    private readonly IClientContextReader _clientContextReader;

    public CartService(BookShopDbContext bookShopDbContext, ILogger<CartService> logger,
                       IMapper mapper, IClientContextReader clientContextReader)
    {
        _bookShopDbContext = bookShopDbContext;
        _logger = logger;
        _mapper = mapper;
        _clientContextReader = clientContextReader;
    }

    public async Task<List<CartItemModel>> GetAllCartItemsAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();

        var cartEntity = await _bookShopDbContext.Carts.Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.ClientId == clientId);

        if (cartEntity == null)
        {
            throw new Exception($"Cart was not found for '{clientId}' client.");
        }

        return _mapper.Map<List<CartItemModel>>(cartEntity.CartItems);
    }

    public async Task<CartItemModel> AddAsync(CartItemAddModel cartItemAddModel)
    {
        if (cartItemAddModel.Count <= 0)
        {
            throw new Exception("Product count cant be less than 0");
        }

        var clientId = _clientContextReader.GetClientContextId();

        var productEntity = await _bookShopDbContext.Products.FirstOrDefaultAsync(p => p.Id == cartItemAddModel.ProductId);

        if (productEntity == null)
        {
            throw new Exception($"Input parametr productId {cartItemAddModel.ProductId} is invalid");
        }

        if (productEntity.Count < cartItemAddModel.Count)
        {
            throw new Exception("Not enough product");
        }

        var cartItemEntity = await _bookShopDbContext.CartItems
            .FirstOrDefaultAsync(c => c.CartEntity.ClientId == clientId &&
                c.ProductId == cartItemAddModel.ProductId);

        if (cartItemEntity != null)
        {
            cartItemEntity.Count += cartItemAddModel.Count;

            await _bookShopDbContext.SaveChangesAsync();
            return _mapper.Map<CartItemModel>(cartItemEntity);
        }

        var cartItemToAdd = _mapper.Map<CartItemEntity>(cartItemAddModel);

        var cartEntity = await _bookShopDbContext.Carts
            .FirstOrDefaultAsync(c => c.ClientId == clientId);

        cartItemToAdd.CartId = cartEntity.Id;

        _bookShopDbContext.CartItems.Add(cartItemToAdd);

        await _bookShopDbContext.SaveChangesAsync();
        _logger.LogInformation($"Product with {cartItemAddModel.ProductId} Id is added in cart for '{clientId}' client.");

        return _mapper.Map<CartItemModel>(cartItemToAdd);
    }

    public async Task<CartItemModel> UpdateAsync(CartItemUpdateModel cartItemUpdateModel)
    {
        if (cartItemUpdateModel.Count <= 0)
        {
            throw new Exception("Product count cant be less than 0");
        }

        var clientId = _clientContextReader.GetClientContextId();

        var cartItemEntity = await _bookShopDbContext.CartItems
            .FirstOrDefaultAsync(ci => ci.CartEntity.ClientId == clientId &&
                ci.ProductId == cartItemUpdateModel.ProductId);

        if (cartItemEntity == null)
        {
            throw new Exception($"There is no Product with {cartItemUpdateModel.ProductId} Id in the Cart.");
        }

        var productEntity = await _bookShopDbContext.Products
            .FirstOrDefaultAsync(p => p.Id == cartItemUpdateModel.ProductId);

        if (productEntity.Count < cartItemUpdateModel.Count)
        {
            throw new Exception("Not enough product");
        }

        cartItemEntity.Count = cartItemUpdateModel.Count;
        await _bookShopDbContext.SaveChangesAsync();

        _logger.LogInformation($"Product with {cartItemUpdateModel.ProductId} Id is updated for '{clientId}' client.");

        return _mapper.Map<CartItemModel>(cartItemEntity);
    }

    public async Task RemoveAsync(long productId)
    {
        var clientId = _clientContextReader.GetClientContextId();

        await _bookShopDbContext.CartItems
            .Where(c => c.CartEntity.ClientId == clientId && c.ProductId == productId)
            .ExecuteDeleteAsync();

        _logger.LogInformation($"Product with {productId} Id is succesfully removed from Cart for '{clientId}' client.");
    }

    public async Task ClearAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();

        await _bookShopDbContext.CartItems
            .Where(c => c.CartEntity.ClientId == clientId)
            .ExecuteDeleteAsync();

        _logger.LogInformation($"CartItems is successfully cleared for '{clientId}' client.");
    }
}