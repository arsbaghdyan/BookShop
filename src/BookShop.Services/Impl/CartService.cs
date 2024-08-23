using AutoMapper;
using BookShop.Common.ClientService.Abstractions;
using BookShop.Data;
using BookShop.Data.Entities;
using BookShop.Repositories.Interfaces;
using BookShop.Services.Abstractions;
using BookShop.Services.Exceptions;
using BookShop.Services.Models.CartItemModels;
using BookShop.Services.Repos.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookShop.Services.Impl;

internal class CartService : ICartService
{
    private readonly IRepository<CartEntity> _cartRepository;
    private readonly IRepository<CartItemEntity> _cartItemRepository;
    private readonly IRepository<ProductEntity> _productRepository;
    private readonly ILogger<CartService> _logger;
    private readonly IMapper _mapper;
    private readonly IClientContextReader _clientContextReader;

    public CartService(IRepository<CartEntity> cartRepository,
                       ILogger<CartService> logger,
                       IMapper mapper,
                       IClientContextReader clientContextReader,
                       IRepository<ProductEntity> productRepository,
                       IRepository<CartItemEntity> cartItemRepository)
    {
        _cartRepository = cartRepository;
        _logger = logger;
        _mapper = mapper;
        _clientContextReader = clientContextReader;
        _productRepository = productRepository;
        _cartItemRepository = cartItemRepository;
    }

    public async Task<List<CartItemModel?>> GetAllCartItemsAsync()
    {
        var clientId = _clientContextReader.GetClientContextId();

        var allCarts = await _cartRepository.GetAllAsync();

        var cartEntity = await allCarts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.ClientId == clientId);

        if (cartEntity == null)
        {
            throw new Exception($"Cart was not found for '{clientId}' client.");
        }

        return _mapper.Map<List<CartItemModel?>>(cartEntity.CartItems);
    }

    public async Task<CartItemModel?> AddAsync(CartItemAddModel cartItemAddModel)
    {
        if (cartItemAddModel.Count <= 0)
        {
            throw new InvalidProductCountException("Product count cant be less or equal 0");
        }

        var clientId = _clientContextReader.GetClientContextId();

        var productEntity = await _productRepository.GetByIdAsync(cartItemAddModel.ProductId);

        if (productEntity == null)
        {
            throw new Exception($"Input parametr productId {cartItemAddModel.ProductId} is invalid");
        }

        if (productEntity.Count < cartItemAddModel.Count)
        {
            throw new NotEnoughProductException("Not enough product");
        }

        var allCartItems = await _cartItemRepository.GetAllAsync();

        var cartItemEntity = await allCartItems
            .Where(c => c.Cart.ClientId == clientId)
            .FirstOrDefaultAsync(c => c.ProductId == cartItemAddModel.ProductId);

        if (cartItemEntity != null)
        {
            if (cartItemEntity.Count + cartItemAddModel.Count < productEntity.Count)
            {
                cartItemEntity.Count += cartItemAddModel.Count;

                await _cartItemRepository.UpdateAsync(cartItemEntity);
                return _mapper.Map<CartItemModel>(cartItemEntity);
            }
            throw new NotEnoughProductException("Not enough product");
        }

        var cartItemToAdd = _mapper.Map<CartItemEntity>(cartItemAddModel);

        var allCarts = await _cartRepository.GetAllAsync();

        cartItemToAdd.CartId = (await allCarts
            .FirstOrDefaultAsync(c => c.ClientId == clientId)).Id;

        await _cartItemRepository.AddAsync(cartItemToAdd);
        _logger.LogInformation($"Product with {cartItemAddModel.ProductId} Id is added in cart for '{clientId}' client.");

        return _mapper.Map<CartItemModel?>(cartItemToAdd);
    }

    public async Task<CartItemModel?> UpdateAsync(CartItemUpdateModel cartItemUpdateModel)
    {
        if (cartItemUpdateModel.Count < 0)
        {
            throw new InvalidProductCountException("Product count cant be less than 0");
        }

        var clientId = _clientContextReader.GetClientContextId();

        var allCartItems = await _cartItemRepository.GetAllAsync();

        var cartItemEntity = await allCartItems
            .Where(ci => ci.Cart.ClientId == clientId)
            .FirstOrDefaultAsync(ci => ci.ProductId == cartItemUpdateModel.ProductId);

        if (cartItemEntity == null)
        {
            throw new Exception($"There is no Product with {cartItemUpdateModel.ProductId} Id in the Cart.");
        }

        if (cartItemUpdateModel.Count == 0)
        {
            await _cartItemRepository.DeleteAsync(cartItemEntity.Id);
            _logger.LogInformation($"Product with {cartItemUpdateModel.ProductId} Id is removed for '{clientId}' client.");

            return null;
        }

        var allProducts = await _productRepository.GetAllAsync();

        var productEntity = await allProducts
            .FirstOrDefaultAsync(p => p.Id == cartItemUpdateModel.ProductId);

        if (productEntity.Count < cartItemUpdateModel.Count)
        {
            throw new NotEnoughProductException("Not enough product");
        }

        cartItemEntity.Count = cartItemUpdateModel.Count;
        await _cartItemRepository.UpdateAsync(cartItemEntity);

        _logger.LogInformation($"Product with {cartItemUpdateModel.ProductId} Id is updated for '{clientId}' client.");

        return _mapper.Map<CartItemModel?>(cartItemEntity);
    }

    public async Task RemoveAsync(long productId)
    {
        var clientId = _clientContextReader.GetClientContextId();

        var allCartItems = await _cartItemRepository.GetAllAsync();

        var cartItemToRemove = await allCartItems.Where(c => c.Cart.ClientId == clientId && c.ProductId == productId)
                                                 .FirstOrDefaultAsync();

        await _cartItemRepository.DeleteAsync(cartItemToRemove.Id);
        _logger.LogInformation($"Product with {productId} Id is succesfully removed from Cart for '{clientId}' client.");
    }
}