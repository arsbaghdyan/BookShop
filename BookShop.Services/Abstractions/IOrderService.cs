using BookShop.Services.Models.OrderModels;

namespace BookShop.Services.Abstractions;

public interface IOrderService
{
    Task<List<OrderModel?>> GetAllAsync();
    Task<OrderModel?> GetByIdAsync(long orderId);
    Task<OrderModelWithPaymentResult?> PlaceOrderFromCartAsync(OrderAddFromCartModel orderAddFromCardModel);
    Task<OrderModelWithPaymentResult?> PlaceOrderAsync(OrderAddModel orderAddModel);
}
