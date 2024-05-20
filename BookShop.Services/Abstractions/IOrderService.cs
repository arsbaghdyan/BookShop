using BookShop.Services.Models.InvoiceModels;
using BookShop.Services.Models.OrderModel;
using BookShop.Services.Models.OrderModels;

namespace BookShop.Services.Abstractions;

public interface IOrderService
{
    Task<List<InvoiceModel>> GetAllAsync();
    Task<InvoiceModel> GetByIdAsync(long paymentId);
    Task<OrderModel> AddOrderFromCartAsync(OrderAddFromCardModel orderAddFromCardModel);
    Task<OrderModel> AddOrderAsync(OrderAddModel orderAddModel);
}
