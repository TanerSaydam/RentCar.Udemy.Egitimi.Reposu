using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Branchs;
using RentCarServer.Domain.Branchs.ValueObjects;
using RentCarServer.Domain.Users;
using TS.MediatR;

namespace RentCarServer.Application.Branches;
public sealed record BranchGetAllQuery : IRequest<IQueryable<BranchGetAllQueryResponse>>;

public sealed class BranchGetAllQueryResponse : EntityDto
{
    public string Name { get; set; } = default!;
    public Address Address { get; set; } = default!;
}

internal sealed class BranchGetAllQueryHandler(
    IBranchRepository branchRepository,
    IUserRepository userRepository) : IRequestHandler<BranchGetAllQuery, IQueryable<BranchGetAllQueryResponse>>
{
    public Task<IQueryable<BranchGetAllQueryResponse>> Handle(BranchGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = branchRepository
            .GetAll()
            .Join(userRepository.GetAll(), m => m.CreatedBy, m => m.Id, (b, user) => new { b = b, user = user })
            .GroupJoin(userRepository.GetAll(), m => m.b.UpdatedBy, m => m.Id, (entity, user) => new { entity = entity, user = user })
            .SelectMany(s => s.user.DefaultIfEmpty(),
                (x, user) => new
                {
                    entity = x.entity,
                    updatedUser = user
                })
            .Select(s => new BranchGetAllQueryResponse
            {
                Id = s.entity.b.Id,
                Name = s.entity.b.Name.Value,
                Address = s.entity.b.Address,
                CreatedAt = s.entity.b.CreatedAt,
                CreatedBy = s.entity.b.CreatedBy,
                IsActive = s.entity.b.IsActive,
                UpdatedAt = s.entity.b.UpdatedAt,
                UpdatedBy = s.entity.b.UpdatedBy,
                CreatedFullName = s.entity.user.FullName.Value,
                UpdatedFullName = s.updatedUser == null ? null : s.updatedUser.FullName.Value
            })
            .AsQueryable();

        return Task.FromResult(response);
    }
}
