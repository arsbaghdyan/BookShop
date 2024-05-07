using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(i => i.Id);
        builder.HasOne(i => i.Client)
            .WithMany(c => c.Invoices)
            .HasForeignKey(i => i.ClientId);
        builder.HasOne(i => i.Payment)
            .WithMany()
            .HasForeignKey(i => i.PaymentId);
        builder.HasOne(i => i.Order)
            .WithMany()
            .HasForeignKey(i => i.OrderId);
    }
}
