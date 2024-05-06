using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.Client)
               .WithMany(pm => pm.PaymentMethods)
               .HasForeignKey(p => p.ClientId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
