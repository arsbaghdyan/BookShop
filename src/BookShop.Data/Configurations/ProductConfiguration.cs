using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BookShop.Data.Entities;

namespace BookShop.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasKey(p => p.Id);
    }
}