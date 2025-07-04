using Microsoft.EntityFrameworkCore;
using RentCarServer.Domain.Branchs;
using RentCarServer.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Branches;
public sealed record BranchGetQuery(
    Guid Id) : IRequest<Result<BranchDto>>;

internal sealed class BranchGetQueryHandler(
    IBranchRepository branchRepository,
    IUserRepository userRepository) : IRequestHandler<BranchGetQuery, Result<BranchDto>>
{
    public async Task<Result<BranchDto>> Handle(BranchGetQuery request, CancellationToken cancellationToken)
    {
        var branch = await branchRepository
            .Where(i => i.Id == request.Id)
            .Join(userRepository.GetAll(), m => m.CreatedBy, m => m.Id, (b, user) => new { b = b, user = user })
            .GroupJoin(userRepository.GetAll(), m => m.b.UpdatedBy, m => m.Id, (entity, user) => new { entity = entity, user = user })
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
            .FirstOrDefaultAsync(cancellationToken);
        if (branch is null)
        {
            return Result<BranchDto>.Failure("Şube bulunamadı");
        }

        return branch;
    }
}