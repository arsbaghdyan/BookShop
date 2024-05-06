using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class WishListItemConfiguration : IEntityTypeConfiguration<WishListItem>
{
    public void Configure(EntityTypeBuilder<WishListItem> builder)
    {
        builder.HasKey(wli => wli.Id);
        builder.HasOne(wli => wli.Client)
            .WithMany(c => c.WishLists)
            .HasForeignKey(wli => wli.ClientId);
        builder.HasOne(wli => wli.Product)
            .WithMany()
            .HasForeignKey(wli => wli.ProductId);
        builder.HasOne(wli => wli.WishList)
            .WithMany(wl => wl.WishListItems)
            .HasForeignKey(wli => wli.WishListId);
    }
}
