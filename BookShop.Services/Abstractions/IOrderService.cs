using BookShop.Services.Models.OrderModels;

namespace BookShop.Services.Abstractions;

public interface IOrderService
{
    Task<List<OrderModel?>> GetAllAsync();
    Task<OrderModel?> GetByIdAsync(long orderId);
    Task<OrderModelWithPaymentResult?> PlaceOrderFromCartAsync(OrderAddFromCardModel orderAddFromCardModel, long paymentMethodId);
    Task<OrderModelWithPaymentResult?> PlaceOrderAsync(OrderAddModel orderAddModel, long paymentMethodId);
}
