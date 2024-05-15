using AutoMapper;
using BookShop.Data.Entities;
using BookShop.Services.Models.CartItemModels;
using BookShop.Services.Models.ClientModels;

namespace BookShop.Services.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ClientRegisterModel, ClientEntity>();
        CreateMap<ClientUpdateModel, ClientEntity>();
        CreateMap<ClientLoginModel, ClientEntity>();

        CreateMap<ClientEntity, ClientModel>();

        CreateMap<ProductAddModel, ProductEntity>();
        CreateMap<ProductUpdateModel, ProductEntity>();
        CreateMap<ProductEntity, ProductModel>();

        CreateMap<CartItemEntity, CartItemModel>();
        CreateMap<CartItemAddModel, CartItemEntity>();
        CreateMap<CartItemUpdateModel, CartItemEntity>();

        CreateMap<WishListItemEntity, WishListItemModel>();
        CreateMap<WishListItemAddModel, WishListItemEntity>();
    }
}
