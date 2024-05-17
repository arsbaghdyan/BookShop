using BookShop.Services.Models.OrderModel;

namespace BookShop.Services.Abstractions;

public interface IOrderService
{
    Task<OrderModel> AddOrderAsync(OrderAddModel orderAddModel);
    Task RemoveAsync(long orderId);
    Task ClearAsync();
}
