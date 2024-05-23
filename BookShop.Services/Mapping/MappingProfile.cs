using AutoMapper;
using BookShop.Data.Entities;
using BookShop.Data.Models;
using BookShop.Services.Models.CartItemModels;
using BookShop.Services.Models.ClientModels;
using BookShop.Services.Models.InvoiceModels;
using BookShop.Services.Models.OrderModels;
using BookShop.Services.Models.PaymentModels;
using static BookShop.Services.Impl.OrderService;

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
        CreateMap<CartItemFromWishListModel, CartItemEntity>();

        CreateMap<WishListItemEntity, WishListItemModel>();
        CreateMap<WishListItemAddModel, WishListItemEntity>();
        CreateMap<WishListItemEntity, CartItemEntity>();
        CreateMap<CartItemEntity, CartItemModel>();

        CreateMap<OrderAddModel, OrderEntity>();
        CreateMap<OrderEntity, OrderModel>();
        CreateMap<OrderEntity, OrderModelWithPaymentResult>();
        CreateMap<OrderInfo, OrderEntity>();
        CreateMap<CartEntity, OrderAddFromCartModel>()
            .ForMember(dest => dest.CartItemId, opt => opt.MapFrom(src => src.CartItems.FirstOrDefault().Id));

        CreateMap<PaymentEntity, PaymentModel>();

        CreateMap<PaymentMethodEntity, BankCardInfo>();
        CreateMap<CardDetails, BankCardInfo>();

        CreateMap<InvoiceEntity, InvoiceModel>();
    }
}
