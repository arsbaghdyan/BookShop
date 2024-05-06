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
               .HasForeignKey(i => i.ClientId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Order)
               .WithOne(o => o.Invoice)
               .HasForeignKey<Invoice>(i => new { i.ClientId, i.Id })
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(i => i.Payments)
               .WithOne(p => p.Invoice)
               .HasForeignKey(p => p.InvoiceId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
