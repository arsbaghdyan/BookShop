using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<CartEntity>
{
    public void Configure(EntityTypeBuilder<CartEntity> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasMany(c => c.CartItems)
               .WithOne(ci => ci.CartEntity)
               .HasForeignKey(ci => ci.CartId);

        builder.HasOne(c => c.ClientEntity)
               .WithOne(c => c.CartEntity)
               .HasForeignKey<CartEntity>(c => c.ClientId);
    }
}