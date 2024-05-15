using BookShop.Services.Models.OrderModel;

namespace BookShop.Services.Abstractions;

public interface IOrderService
{
    Task<OrderModel> AddOrder(OrderAddModel orderAddModel);

}
