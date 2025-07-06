using GenericRepository;
using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.Extras;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Extras;

public sealed record ExtraDeleteCommand(Guid Id) : IRequest<Result<string>>;
[Permission("extra:delete")]
internal sealed class ExtraDeleteCommandHandler(
    IExtraRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<ExtraDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ExtraDeleteCommand request, CancellationToken cancellationToken)
    {
        var extra = await repository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (extra is null)
            return Result<string>.Failure("Ekstra bulunamadý");

        extra.Delete();
        repository.Update(extra);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Ekstra baþarýyla silindi";
    }
}