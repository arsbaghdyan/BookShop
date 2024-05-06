using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BookShop.Data.Entities;

namespace BookShop.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasMany(c => c.WishLists)
               .WithOne(w => w.Product)
               .HasForeignKey(w => w.ClientId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Carts)
               .WithOne(c => c.Product)
               .HasForeignKey(c => c.ClientId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Orders)
               .WithOne(o => o.Product)
               .HasForeignKey(o => o.ClientId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
