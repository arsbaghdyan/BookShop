using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasMany(c => c.PaymentMethods)
            .WithOne(pm => pm.Client)
            .HasForeignKey(pm => pm.ClientId);
        builder.HasMany(c => c.WishLists)
            .WithOne(wl => wl.Client)
            .HasForeignKey(wl => wl.ClientId);
        builder.HasMany(c => c.CartItems)
            .WithOne(ci => ci.Client)
            .HasForeignKey(ci => ci.ClientId);
        builder.HasMany(c => c.Orders)
            .WithOne(o => o.Client)
            .HasForeignKey(o => o.ClientId);
        builder.HasMany(c => c.Invoices)
            .WithOne(i => i.Client)
            .HasForeignKey(i => i.ClientId);
    }
}
