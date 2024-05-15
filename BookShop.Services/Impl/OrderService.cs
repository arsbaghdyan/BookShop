using BookShop.Services.Abstractions;
using BookShop.Services.Models.OrderModel;

namespace BookShop.Services.Impl;

internal class OrderService : IOrderService
{
    public Task<OrderModel> AddOrder(OrderAddModel orderAddModel)
    {
        throw new NotImplementedException();
    }
}
