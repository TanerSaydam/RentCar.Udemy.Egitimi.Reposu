using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Roles;

namespace RentCarServer.Application.Roles;
public sealed class RoleDto : EntityDto
{
    public string Name { get; set; } = default!;
    public int PermissionCount { get; set; }
    public List<string> Permissions { get; set; } = new();
}

public static class RoleExtensions
{
    public static IQueryable<RoleDto> MapTo(this IQueryable<EntityWithAuditDto<Role>> entites)
    {
        var res = entites.Select(s => new RoleDto
        {
            Id = s.Entity.Id,
            Name = s.Entity.Name.Value,
            PermissionCount = s.Entity.Permissions.Count,
            IsActive = s.Entity.IsActive,
            CreatedAt = s.Entity.CreatedAt,
            CreatedBy = s.Entity.CreatedBy,
            CreatedFullName = s.CreatedUser.FullName.Value,
            UpdatedAt = s.Entity.UpdatedAt,
            UpdatedBy = s.Entity.UpdatedBy != null ? s.Entity.UpdatedBy.Value : null,
            UpdatedFullName = s.UpdatedUser != null ? s.UpdatedUser.FullName.Value : null,
        }).AsQueryable();

        return res;
    }

    public static IQueryable<RoleDto> MapToGet(this IQueryable<EntityWithAuditDto<Role>> entites)
    {
        var res = entites.Select(s => new RoleDto
        {
            Id = s.Entity.Id,
            Name = s.Entity.Name.Value,
            PermissionCount = s.Entity.Permissions.Count,
            Permissions = s.Entity.Permissions.Select(s => s.Value).ToList(),
            IsActive = s.Entity.IsActive,
            CreatedAt = s.Entity.CreatedAt,
            CreatedBy = s.Entity.CreatedBy,
            CreatedFullName = s.CreatedUser.FullName.Value,
            UpdatedAt = s.Entity.UpdatedAt,
            UpdatedBy = s.Entity.UpdatedBy != null ? s.Entity.UpdatedBy.Value : null,
            UpdatedFullName = s.UpdatedUser != null ? s.UpdatedUser.FullName.Value : null,
        }).AsQueryable();

        return res;
    }
}
