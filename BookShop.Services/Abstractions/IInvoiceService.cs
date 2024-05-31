using BookShop.Data.Entities;
using BookShop.Services.Models.InvoiceModels;

namespace BookShop.Services.Abstractions;

public interface IInvoiceService
{
    Task<InvoiceModel?> GetByIdAsync(long orderId);
    Task<InvoiceModel> CreateInvoiceAsync(OrderEntity orderEntity);
}
