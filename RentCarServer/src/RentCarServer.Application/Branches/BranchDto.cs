using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Branchs.ValueObjects;

namespace RentCarServer.Application.Branches;
public sealed class BranchDto : EntityDto
{
    public string Name { get; set; } = default!;
    public Address Address { get; set; } = default!;
}

public static class BranchExtensions
{
    public static IQueryable<BranchDto> MapTo(this IQueryable<EntityWithAuditDto<Branch>> entity)
    {
        var res = entity
            .Select(s => new BranchDto
            {
                Id = s.Entity.Id,
                Name = s.Entity.Name.Value,
                Address = s.Entity.Address,
                CreatedAt = s.Entity.CreatedAt,
                CreatedBy = s.Entity.CreatedBy,
                IsActive = s.Entity.IsActive,
                UpdatedAt = s.Entity.UpdatedAt,
                UpdatedBy = s.Entity.UpdatedBy == null ? null : s.Entity.UpdatedBy.Value,
                CreatedFullName = s.CreatedUser.FullName.Value,
                UpdatedFullName = s.UpdatedUser == null ? null : s.UpdatedUser.FullName.Value
            })
            .AsQueryable();

        return res;
    }
}