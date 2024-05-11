using AutoMapper;
using BookShop.Data.Entities;
using BookShop.Services.Models.CartItemModels;

namespace BookShop.Services.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ClientRegisterVm, ClientEntity>();
        CreateMap<ClientUpdateVm, ClientEntity>();
        CreateMap<ClientLoginVm, ClientEntity>();

        CreateMap<ProductAddVm, ProductEntity>();
        CreateMap<ProductUpdateVm, ProductEntity>();
        CreateMap<ProductEntity, ProductGetVm>();

        CreateMap<CartItemEntity, CartItemGetVm>();
        CreateMap<CartItemAddVm, CartItemEntity>();
        CreateMap<CartItemUpdateVm, CartItemEntity>();

        CreateMap<WishListItemEntity, WishListItemGetVm>();
        CreateMap<WishListItemAddVm, WishListItemEntity>();

        CreateMap<PaymentMethodAddVm, PaymentMethodEntity>();
        CreateMap<PaymentMethodGetVm, PaymentMethodEntity>();
    }
}
