using BookShop.Data.Abstractions;

namespace BookShop.Data.Entities;

public class Client : IIdentifiable
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public List<PaymentMethod> PaymentMethods { get; set; } = new();
    public List<WishListItem> WishLists { get; set; } = new();
    public List<CartItem> CartItems { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
    public List<Invoice> Invoices { get; set; } = new();
}