using AutoMapper;
using BookShop.Data.Entities;
using BookShop.Data.Models;
using BookShop.Services.Models.BillingModels;
using BookShop.Services.Models.CartItemModels;
using BookShop.Services.Models.ClientModels;
using BookShop.Services.Models.EmployeeModels;
using BookShop.Services.Models.InvoiceModels;
using BookShop.Services.Models.OrderModels;
using BookShop.Services.Models.OrderProductModels;
using BookShop.Services.Models.PaymentMethodModels;
using BookShop.Services.Models.PaymentModels;
using BookShop.Services.Models.ProductModels;
using BookShop.Services.Models.WishListItemModels;
using static BookShop.Services.Impl.OrderService;

namespace BookShop.Services.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ClientRegisterModel, ClientEntity>();
        CreateMap<ClientUpdateModel, ClientEntity>();
        CreateMap<ClientEntity, ClientModel>();

        CreateMap<ProductAddModel, ProductEntity>();
        CreateMap<ProductUpdateModel, ProductEntity>();
        CreateMap<ProductEntity, ProductModel>();

        CreateMap<CartItemEntity, CartItemModel>();
        CreateMap<CartItemAddModel, CartItemEntity>();
        CreateMap<CartItemUpdateModel, CartItemEntity>();

        CreateMap<WishListItemEntity, WishListItemModel>();
        CreateMap<WishListItemAddModel, WishListItemEntity>();
        CreateMap<WishListItemEntity, CartItemEntity>();

        CreateMap<OrderAddModel, OrderEntity>();
        CreateMap<OrderEntity, OrderModel>()
            .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src => src.OrderProducts))
            .ForMember(dest => dest.InvoiceId, opt => opt.MapFrom(src => src.Invoice.Id))
            .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Invoice.Payments.FirstOrDefault().Id));
        CreateMap<OrderEntity, OrderModelWithPaymentResult>();
        CreateMap<OrderInfo, OrderEntity>();

        CreateMap<CartEntity, OrderAddFromCartModel>()
            .ForMember(dest => dest.CartItemIds, opt => opt.MapFrom(src => src.CartItems.FirstOrDefault().Id));

        CreateMap<OrderProduct, OrderProductModel>();
        CreateMap<PaymentEntity, PaymentModel>();

        CreateMap<PaymentMethodEntity, BankCardInfo>();
        CreateMap<CardDetails, BankCardInformation>();

        CreateMap<InvoiceEntity, InvoiceModel>();

        CreateMap<EmployeeRegisterModel, EmployeeEntity>();
        CreateMap<EmployeeUpdateModel, EmployeeEntity>();
        CreateMap<EmployeeEntity, EmployeeModel>();
    }
}
