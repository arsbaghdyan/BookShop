using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<PaymentEntity>
{
    public void Configure(EntityTypeBuilder<PaymentEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.PaymentMethodEntity)
               .WithMany(pm => pm.Payments)
               .HasForeignKey(p => p.PaymentMethodId);
    }
}