using AutoMapper;
using BookShop.Data.Entities;
using BookShop.Services.Models.CartItemModels;
using BookShop.Services.Models.ClientModels;
using BookShop.Services.Models.InvoiceModels;
using BookShop.Services.Models.OrderModel;
using BookShop.Services.Models.PaymentModels;

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

        CreateMap<OrderEntity, OrderModel>();
        CreateMap<OrderAddModel, OrderEntity>();

        CreateMap<PaymentEntity, PaymentModel>();
        CreateMap<PaymentAddModel, PaymentEntity>();

        CreateMap<PaymentMethodEntity, PaymentMethodModel>();
        CreateMap<PaymentMethodAddModel, PaymentMethodEntity>();

        CreateMap<InvoiceEntity, InvoiceModel>();
    }
}
