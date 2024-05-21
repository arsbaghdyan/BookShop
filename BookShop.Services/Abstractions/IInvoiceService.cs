using BookShop.Services.Models.InvoiceModels;

namespace BookShop.Services.Abstractions;

public interface IInvoiceService
{
    Task<InvoiceModel?> GetByIdAsync(long invoiceId);
}
