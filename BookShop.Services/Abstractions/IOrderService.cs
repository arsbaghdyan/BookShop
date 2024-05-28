using BookShop.Services.Models.OrderModels;

namespace BookShop.Services.Abstractions;

public interface IOrderService
{
    Task<List<OrderModel?>> GetAllAsync();
    Task<OrderModel?> GetByIdAsync(long orderId);
    Task<List<OrderModelWithPaymentResult>?> PlaceOrderAsync(List<OrderAddModel> orderAddModels);
    Task<List<OrderModelWithPaymentResult>?> PlaceOrderFromCartAsync(List<OrderAddFromCartModel> orderAddFromCardModels);
}
