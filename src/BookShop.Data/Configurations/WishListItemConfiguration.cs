using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class WishListItemConfiguration : IEntityTypeConfiguration<WishListItemEntity>
{
    public void Configure(EntityTypeBuilder<WishListItemEntity> builder)
    {
        builder.HasKey(wli => wli.Id);

        builder.HasOne(wli => wli.Product)
               .WithMany(p => p.WishListItems)
               .HasForeignKey(wli => wli.ProductId);

        builder.HasOne(wli => wli.WishList)
               .WithMany(wl => wl.WishListItems)
               .HasForeignKey(wli => wli.WishListId);

        builder.HasIndex(w => new { w.WishListId, w.ProductId }).IsUnique();
    }
}