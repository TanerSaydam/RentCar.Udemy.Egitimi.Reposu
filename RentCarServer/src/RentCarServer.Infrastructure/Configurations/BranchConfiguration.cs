using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCarServer.Domain.Branches;

namespace RentCarServer.Infrastructure.Configurations;
internal sealed class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.HasKey(x => x.Id);
        builder.OwnsOne(i => i.Name);
        builder.OwnsOne(i => i.Address);
    }
}
