using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasIndex(c => new { c.ClientId, c.ProductId });

        builder.HasOne(c => c.Client)
               .WithMany(c => c.Carts)
               .HasForeignKey(c => c.ClientId);

        builder.HasOne(c => c.Product)
               .WithMany(p => p.Carts)
               .HasForeignKey(c => c.ProductId);
    }
}