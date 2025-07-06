using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Roles;
using RentCarServer.Domain.Users;

namespace RentCarServer.Application.Users;
public sealed class UserDto : EntityDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string BranchName { get; set; } = default!;
    public string RoleName { get; set; } = default!;
}

public static class UserExtensions
{
    public static IQueryable<UserDto> MapTo(
        this IQueryable<EntityWithAuditDto<User>> entities,
        IQueryable<Role> roles,
        IQueryable<Branch> branches
        )
    {
        var res = entities
            .Join(roles, m => m.Entity.RoleId, m => m.Id, (e, role)
                => new { e.Entity, e.CreatedUser, e.UpdatedUser, Role = role })
            .Join(branches, m => m.Entity.BranchId, m => m.Id, (entity, branch)
                => new { entity.Entity, entity.CreatedUser, entity.UpdatedUser, entity.Role, Branch = branch })
            .Select(s => new UserDto
            {
                Id = s.Entity.Id,
                FirstName = s.Entity.FirstName.Value,
                LastName = s.Entity.LastName.Value,
                FullName = s.Entity.FullName.Value,
                Email = s.Entity.Email.Value,
                UserName = s.Entity.UserName.Value,
                RoleName = s.Role.Name.Value,
                BranchName = s.Branch.Name.Value,
                IsActive = s.Entity.IsActive,
                CreatedAt = s.Entity.CreatedAt,
                CreatedBy = s.Entity.CreatedBy.Value,
                CreatedFullName = s.CreatedUser.FullName.Value,
                UpdatedAt = s.Entity.UpdatedAt,
                UpdatedBy = s.Entity.UpdatedBy != null ? s.Entity.UpdatedBy.Value : null,
                UpdatedFullName = s.UpdatedUser != null ? s.UpdatedUser.FullName.Value : null,
            });

        return res;
    }
}