using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class ClientEntity : IIdentifiable
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public WishListEntity? WishListEntity { get; set; }
    public CartEntity? CartEntity { get; set; }
    public List<PaymentMethodEntity> PaymentMethods { get; set; } = new();
    public List<OrderEntity> Orders { get; set; } = new();
    public List<InvoiceEntity> Invoices { get; set; } = new();
}