using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.HasKey(o => o.Id);

        builder.HasOne(o => o.ProductEntity)
               .WithMany(p => p.Orders)
               .HasForeignKey(o => o.ProductId);
    }
}