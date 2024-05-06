using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class CardDetailsConfiguration : IEntityTypeConfiguration<CardDetails>
{
    public void Configure(EntityTypeBuilder<CardDetails> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.CardNumber).IsRequired();
        builder.Property(c => c.CVV).IsRequired();
        builder.Property(c => c.ExpiredAt).IsRequired();
        builder.Property(c => c.FirstName).IsRequired();
        builder.Property(c => c.LastName).IsRequired();
        builder.Ignore(c => c.FullName);
    }
}
