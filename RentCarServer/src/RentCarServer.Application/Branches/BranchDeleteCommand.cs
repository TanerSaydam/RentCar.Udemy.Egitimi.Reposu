using GenericRepository;
using RentCarServer.Domain.Branches;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Branches;
public sealed record BranchDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class BranchDeleteCommandHandler(
    IBranchRepository branchRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<BranchDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(BranchDeleteCommand request, CancellationToken cancellationToken)
    {
        var branch = await branchRepository.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        if (branch is null)
        {
            return Result<string>.Failure("Şube bulunamadı");
        }

        branch.Delete();
        branchRepository.Update(branch);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Şube başarıyla silindi";
    }
}