using AutoMapper;
using BookShop.Data.Entities;
using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ClientRegisterModel, ClientEntity>();
        CreateMap<ClientUpdateModel, ClientEntity>();
        CreateMap<ClientLoginModel, ClientEntity>();
        CreateMap<ClientDeleteModel, ClientEntity>();

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

        CreateMap<PaymentMethodAddModel, PaymentMethodEntity>();
        CreateMap<PaymentMethodGetModel, PaymentMethodEntity>();
        CreateMap<PaymentMethodDeleteModel, PaymentMethodEntity>();
    }
}
