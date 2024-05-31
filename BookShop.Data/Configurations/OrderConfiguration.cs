using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.HasKey(o => o.Id);

        builder.HasOne(o => o.PaymentMethod)
               .WithMany(pm => pm.Orders)
               .HasForeignKey(o => o.PaymentMethodId);

        builder.HasMany(o => o.OrderProducts)
               .WithOne(op => op.Order)
               .HasForeignKey(o => o.OrderId);
    }
}