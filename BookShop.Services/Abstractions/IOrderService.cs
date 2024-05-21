using BookShop.Services.Models.InvoiceModels;
using BookShop.Services.Models.OrderModel;
using BookShop.Services.Models.OrderModels;

namespace BookShop.Services.Abstractions;

public interface IOrderService
{
    Task<List<OrderModel>> GetAllAsync();
    Task<OrderModel> GetByIdAsync(long orderId);
    Task<OrderModel> PlaceOrderFromCartAsync(OrderAddFromCardModel orderAddFromCardModel);
    Task<OrderModel> PlaceOrderAsync(OrderAddModel orderAddModel);
}
