using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Data;

public partial class BookShopDbContext : DbContext
{
    public BookShopDbContext(DbContextOptions<BookShopDbContext> options) : base(options)
    {

    }

    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<WishListItem> WishListItems { get; set; }
    public DbSet<WishList> WishLists { get; set; }
    public DbSet<CardDetails> CardsDetails { get; set; }
}
