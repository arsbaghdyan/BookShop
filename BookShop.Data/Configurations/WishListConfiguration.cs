using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class WishListConfiguration : IEntityTypeConfiguration<WishListItem>
{
    public void Configure(EntityTypeBuilder<WishListItem> builder)
    {
        builder.HasKey(w => w.Id);

        builder.HasIndex(w => new { w.ClientId, w.ProductId });

        builder.HasOne(w => w.Client)
               .WithMany(c => c.WishLists)
               .HasForeignKey(w => w.ClientId);

        builder.HasOne(w => w.Product)
               .WithMany(p => p.WishLists)
               .HasForeignKey(w => w.ProductId);
    }
}
