using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItemEntity>
{
    public void Configure(EntityTypeBuilder<CartItemEntity> builder)
    {
        builder.HasKey(ci => ci.Id);

        builder.HasOne(ci => ci.Product)
               .WithMany(p => p.CartItems)
               .HasForeignKey(ci => ci.ProductId);

        builder.HasOne(ci => ci.Cart)
               .WithMany(c => c.CartItems)
               .HasForeignKey(ci => ci.CartId);

        builder.HasIndex(w => new { w.CartId, w.ProductId }).IsUnique();
    }
}