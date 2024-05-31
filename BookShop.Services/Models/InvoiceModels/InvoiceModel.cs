using BookShop.Data.Enums;

namespace BookShop.Services.Models.InvoiceModels;

public class InvoiceModel
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public long OrderId { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public InvoiceStatus InvoiceStatus { get; set; }
}
