using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<ClientEntity>
{
    public void Configure(EntityTypeBuilder<ClientEntity> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.Email)
               .IsUnique();

        builder.HasMany(c => c.PaymentMethods)
               .WithOne(pm => pm.ClientEntity)
               .HasForeignKey(pm => pm.ClientId);

        builder.HasMany(c => c.Orders)
               .WithOne(o => o.ClientEntity)
               .HasForeignKey(o => o.ClientId);

        builder.HasMany(c => c.Invoices)
               .WithOne(i => i.ClientEntity)
               .HasForeignKey(i => i.ClientId);

        builder.HasOne(w => w.WishListEntity)
               .WithOne(c => c.ClientEntity)
               .HasForeignKey<WishListEntity>(w => w.ClientId);
    }
}