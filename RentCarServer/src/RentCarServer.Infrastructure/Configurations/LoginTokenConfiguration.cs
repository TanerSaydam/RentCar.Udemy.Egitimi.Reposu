using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCarServer.Domain.LoginTokens;

namespace RentCarServer.Infrastructure.Configurations;
internal sealed class LoginTokenConfiguration : IEntityTypeConfiguration<LoginToken>
{
    public void Configure(EntityTypeBuilder<LoginToken> builder)
    {
        builder.ToTable("LoginTokens");
        builder.HasKey(x => x.Id);
        builder.OwnsOne(p => p.Token);
        builder.OwnsOne(p => p.IsActive);
        builder.OwnsOne(p => p.ExpiresDate);
    }
}
