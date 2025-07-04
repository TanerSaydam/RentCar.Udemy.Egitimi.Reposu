using RentCarServer.Domain.Branchs;
using RentCarServer.Domain.Users;
using TS.MediatR;

namespace RentCarServer.Application.Branches;
public sealed record BranchGetAllQuery : IRequest<IQueryable<BranchDto>>;

internal sealed class BranchGetAllQueryHandler(
    IBranchRepository branchRepository,
    IUserRepository userRepository) : IRequestHandler<BranchGetAllQuery, IQueryable<BranchDto>>
{
    public Task<IQueryable<BranchDto>> Handle(BranchGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = branchRepository
            .GetAll()
            .MapTo(userRepository.GetAll());

        return Task.FromResult(response);
    }
}
