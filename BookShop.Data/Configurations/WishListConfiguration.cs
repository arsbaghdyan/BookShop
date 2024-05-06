using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class WishListConfiguration : IEntityTypeConfiguration<WishList>
{
    public void Configure(EntityTypeBuilder<WishList> builder)
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
