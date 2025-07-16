using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCarServer.Domain.Customers;

namespace RentCarServer.Infrastructure.Configurations;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.FirstName);
        builder.OwnsOne(x => x.LastName);
        builder.OwnsOne(x => x.FullName);
        builder.OwnsOne(x => x.IdentityNumber);
        builder.OwnsOne(x => x.DateOfBirth);
        builder.OwnsOne(x => x.PhoneNumber);
        builder.OwnsOne(x => x.Email);
        builder.OwnsOne(x => x.DrivingLicenseIssuanceDate);
        builder.OwnsOne(x => x.FullAddress);
        builder.OwnsOne(x => x.Password);
    }
}