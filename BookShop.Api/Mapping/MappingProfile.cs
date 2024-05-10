using AutoMapper;
using BookShop.Api.Models.CartItemModels;
using BookShop.Api.Models.CartModel;
using BookShop.Api.Models.ClientModels;
using BookShop.Api.Models.ProductModels;
using BookShop.Api.Models.WishListItemModels;
using BookShop.Api.Models.WishListModels;
using BookShop.Data.Entities;

namespace BookShop.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ClientRegisterModel, ClientEntity>();
        CreateMap<ClientUpdateModel, ClientEntity>();
        CreateMap<ClientLoginModel, ClientEntity>();

        CreateMap<ProductAddModel, ProductEntity>();
        CreateMap<ProductUpdateModel, ProductEntity>();
        CreateMap<ProductEntity, ProductGetModel>();

        CreateMap<CartCreateModel, CartEntity>();

        CreateMap<CartItemEntity, CartItemGetModel>();
        CreateMap<CartItemAddModel, CartItemEntity>();
        CreateMap<CartItemDeleteModel, CartItemEntity>();
        CreateMap<CartItemUpdateModel, CartItemEntity>();

        CreateMap<WishListCreateModel, WishListEntity>();

        CreateMap<WishListItemEntity, WishListItemGetModel>();
        CreateMap<WishListItemAddModel, WishListItemEntity>();
        CreateMap<WishListItemDeleteModel, WishListItemEntity>();
    }
}
