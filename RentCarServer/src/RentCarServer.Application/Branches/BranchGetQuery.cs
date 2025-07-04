using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Branchs;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Branches;
public sealed record BranchGetQuery(
    Guid Id) : IRequest<Result<Branch>>;

internal sealed class BranchGetQueryHandler(
    IBranchRepository branchRepository) : IRequestHandler<BranchGetQuery, Result<Branch>>
{
    public async Task<Result<Branch>> Handle(BranchGetQuery request, CancellationToken cancellationToken)
    {
        var branch = await branchRepository.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        if (branch is null)
        {
            return Result<Branch>.Failure("Şube bulunamadı");
        }

        return branch;
    }
}