using AutoMapper;
using BookShop.Api.Models.ClientModels;
using BookShop.Api.Models.ProductModels;
using BookShop.Data.Entities;

namespace BookShop.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ClientRegisterModel, ClientEntity>();
        CreateMap<ClientRemoveModel, ClientEntity>();
        CreateMap<ClientUpdateModel, ClientEntity>();

        CreateMap<ProductAddModel, ProductEntity>();
        CreateMap<ProductRemoveModel, ProductEntity>();
        CreateMap<ProductUpdateModel, ProductEntity>();
    }
}
