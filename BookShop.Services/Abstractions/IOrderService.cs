using BookShop.Services.Models.OrderModel;
using BookShop.Services.Models.OrderModels;

namespace BookShop.Services.Abstractions;

public interface IOrderService
{
    Task<OrderModel> AddOrderAsync(OrderAddModel orderAddModel);
    Task<OrderModel> AddOrderFromCartAsync(OrderAddFromCardModel orderAddFromCardModel);
    Task RemoveAsync(long orderId);
    Task ClearAsync();
}
