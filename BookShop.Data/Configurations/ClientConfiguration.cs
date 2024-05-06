using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasMany(c => c.WishLists)
               .WithOne(w => w.Client)
               .HasForeignKey(w => w.ClientId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Carts)
               .WithOne(c => c.Client)
               .HasForeignKey(c => c.ClientId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Orders)
               .WithOne(o => o.Client)
               .HasForeignKey(o => o.ClientId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
