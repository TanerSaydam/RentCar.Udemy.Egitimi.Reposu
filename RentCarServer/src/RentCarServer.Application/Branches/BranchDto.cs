using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Branchs.ValueObjects;
using RentCarServer.Domain.Users;

namespace RentCarServer.Application.Branches;
public sealed class BranchDto : EntityDto
{
    public string Name { get; set; } = default!;
    public Address Address { get; set; } = default!;
}

public static class BranchExtensions
{
    public static IQueryable<BranchDto> MapTo(this IQueryable<Branch> entity, IQueryable<User> users)
    {
        var res = entity
            .Join(users, m => m.CreatedBy, m => m.Id, (b, user) => new { b = b, user = user })
            .GroupJoin(users, m => m.b.UpdatedBy, m => m.Id, (entity, user) => new { entity = entity, user = user })
            .SelectMany(s => s.user.DefaultIfEmpty(),
                (x, user) => new
                {
                    entity = x.entity,
                    updatedUser = user
                })
            .Select(s => new BranchDto
            {
                Id = s.entity.b.Id,
                Name = s.entity.b.Name.Value,
                Address = s.entity.b.Address,
                CreatedAt = s.entity.b.CreatedAt,
                CreatedBy = s.entity.b.CreatedBy,
                IsActive = s.entity.b.IsActive,
                UpdatedAt = s.entity.b.UpdatedAt,
                UpdatedBy = s.entity.b.UpdatedBy == null ? null : s.entity.b.UpdatedBy.Value,
                CreatedFullName = s.entity.user.FullName.Value,
                UpdatedFullName = s.updatedUser == null ? null : s.updatedUser.FullName.Value
            })
            .AsQueryable();

        return res;
    }
}