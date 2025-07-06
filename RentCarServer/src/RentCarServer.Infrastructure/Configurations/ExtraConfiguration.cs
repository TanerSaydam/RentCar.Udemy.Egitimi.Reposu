using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCarServer.Domain.Extras;

namespace RentCarServer.Infrastructure.Configurations;

internal sealed class ExtraConfiguration : IEntityTypeConfiguration<Extra>
{
    public void Configure(EntityTypeBuilder<Extra> builder)
    {
        builder.ToTable("Extras");
        builder.HasKey(x => x.Id);
        builder.OwnsOne(x => x.Name);
        builder.OwnsOne(x => x.Price);
        builder.OwnsOne(x => x.Description);
    }
}