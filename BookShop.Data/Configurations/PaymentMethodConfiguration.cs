using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.HasKey(pm => pm.Id);
        builder.HasOne(pm => pm.Client)
            .WithMany(c => c.PaymentMethods)
            .HasForeignKey(pm => pm.ClientId);
        builder.HasOne(pm => pm.Details);
    }
}
