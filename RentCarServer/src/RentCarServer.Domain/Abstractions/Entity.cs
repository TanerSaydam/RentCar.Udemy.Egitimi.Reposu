using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RentCarServer.Domain.Users;

namespace RentCarServer.Domain.Abstractions;
public abstract class Entity
{
    protected Entity()
    {
        Id = new IdentityId(Guid.CreateVersion7());
        IsActive = true;
    }

    public IdentityId Id { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public IdentityId CreatedBy { get; private set; } = default!;

    public string CreatedFullName => GetCreatedFullName();
    public DateTimeOffset? UpdatedAt { get; private set; }
    public IdentityId? UpdatedBy { get; private set; }
    public string? UpdatedFullName => GetUpdatedFullName();
    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public IdentityId? DeletedBy { get; private set; }

    public void SetStatus(bool isActive)
    {
        IsActive = isActive;
    }

    public void Delete()
    {
        IsDeleted = true;
    }

    private string GetCreatedFullName()
    {
        HttpContextAccessor httpContextAccessor = new();
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return string.Empty;
        }

        var srv = httpContext.RequestServices;
        using var scoped = srv.CreateScope();
        var userRepository = scoped.ServiceProvider.GetService<IUserRepository>();
        if (userRepository is null) return string.Empty;

        var userFullName = userRepository.FirstOrDefault(i => i.Id == CreatedBy).FullName;
        if (userFullName is null) return string.Empty;

        return userFullName.Value;
    }

    private string? GetUpdatedFullName()
    {
        if (UpdatedBy is not null)
        {
            HttpContextAccessor httpContextAccessor = new();
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext is null)
            {
                return string.Empty;
            }

            var srv = httpContext.RequestServices;
            using var scoped = srv.CreateScope();
            var userRepository = scoped.ServiceProvider.GetService<IUserRepository>();
            if (userRepository is null) return string.Empty;

            var userFullName = userRepository.FirstOrDefault(i => i.Id == UpdatedBy).FullName;
            if (userFullName is null) return string.Empty;

            return userFullName.Value;
        }

        return null;
    }
}

public sealed record IdentityId(Guid Value)
{
    public static implicit operator Guid(IdentityId id) => id.Value;
    public static implicit operator string(IdentityId id) => id.Value.ToString();
}
