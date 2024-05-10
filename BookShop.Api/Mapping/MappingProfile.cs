using AutoMapper;
using BookShop.Api.Models.CartItemModels;
using BookShop.Api.Models.CartModel;
using BookShop.Api.Models.ClientModels;
using BookShop.Api.Models.ProductModels;
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

    }
}
