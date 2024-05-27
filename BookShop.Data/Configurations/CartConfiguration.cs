using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<CartEntity>
{
    public void Configure(EntityTypeBuilder<CartEntity> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.Client)
               .WithOne(c => c.Cart)
               .HasForeignKey<CartEntity>(c => c.ClientId);
    }
}