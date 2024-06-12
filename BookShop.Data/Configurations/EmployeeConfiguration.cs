using BookShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookShop.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasData(new EmployeeEntity 
        {
            Id=1,
            FirstName="Admin",
            LastName="Adminyan",
            Address="New York",
            Email="admin",
            Password="admin",
            Position="Admin",
            Salary=10000
        });

        builder.HasIndex(e => e.Email)
               .IsUnique();
    }
}
