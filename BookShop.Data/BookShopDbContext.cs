using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Data;

public partial class BookShopDbContext : DbContext
{
    public BookShopDbContext(DbContextOptions<BookShopDbContext> options) : base(options)
    {

    }

    public DbSet<CartEntity> Carts { get; set; }
    public DbSet<CartItemEntity> CartItems { get; set; }
    public DbSet<ClientEntity> Clients { get; set; }
    public DbSet<InvoiceEntity> Invoices { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<PaymentEntity> Payments { get; set; }
    public DbSet<PaymentMethodEntity> PaymentMethods { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<WishListItemEntity> WishListItems { get; set; }
    public DbSet<WishListEntity> WishLists { get; set; }
}