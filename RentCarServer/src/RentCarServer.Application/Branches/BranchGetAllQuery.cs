using RentCarServer.Domain.Branches;
using TS.MediatR;

namespace RentCarServer.Application.Branches;
public sealed record BranchGetAllQuery : IRequest<IQueryable<BranchDto>>;

internal sealed class BranchGetAllQueryHandler(
    IBranchRepository branchRepository) : IRequestHandler<BranchGetAllQuery, IQueryable<BranchDto>>
{
    public Task<IQueryable<BranchDto>> Handle(BranchGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = branchRepository
            .GetAllWithAudit()
            .MapTo();

        return Task.FromResult(response);
    }
}
