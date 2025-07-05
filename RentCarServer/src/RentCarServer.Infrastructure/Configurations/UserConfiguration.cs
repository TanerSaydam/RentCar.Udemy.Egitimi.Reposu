using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCarServer.Domain.Users;

namespace RentCarServer.Infrastructure.Configurations;
internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(i => i.Id);
        builder.OwnsOne(i => i.FirstName);
        builder.OwnsOne(i => i.LastName);
        builder.OwnsOne(i => i.FullName);
        builder.OwnsOne(i => i.Email);
        builder.OwnsOne(i => i.UserName);
        builder.OwnsOne(i => i.Password);
        builder.OwnsOne(i => i.ForgotPasswordCode);
        builder.OwnsOne(i => i.ForgotPasswordDate);
        builder.OwnsOne(i => i.IsForgotPasswordCompleted);
        builder.OwnsOne(i => i.TFAStatus);
        builder.OwnsOne(i => i.TFACode);
        builder.OwnsOne(i => i.TFAConfirmCode);
        builder.OwnsOne(i => i.TFAExpiresDate);
        builder.OwnsOne(i => i.TFAIsCompleted);
    }
}
