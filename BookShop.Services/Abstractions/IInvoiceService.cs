using BookShop.Services.Models.InvoiceModels;

namespace BookShop.Services.Abstractions;

public interface IInvoiceService
{
    Task<List<InvoiceModel>> GetAllAsync();
    Task<InvoiceModel> GetByIdAsync(long paymentId);
    Task ClearAsync();
}
