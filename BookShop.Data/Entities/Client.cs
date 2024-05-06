using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class Client : IIdentifiable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public List<PaymentMethod>? PaymentMethods { get; set; }
    public List<WishList>? WishLists { get; set; }
    public List<Cart>? Carts { get; set; }
    public List<Order>? Orders { get; set; }
    public List<Invoice>? Invoices { get; set; }
}