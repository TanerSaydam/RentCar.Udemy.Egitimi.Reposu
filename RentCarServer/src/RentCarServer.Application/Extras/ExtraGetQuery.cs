using Microsoft.EntityFrameworkCore;
using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.Extras;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Extras;

public sealed record ExtraGetQuery(Guid Id) : IRequest<Result<ExtraDto>>;
[Permission("extra:view")]
internal sealed class ExtraGetQueryHandler(
    IExtraRepository repository) : IRequestHandler<ExtraGetQuery, Result<ExtraDto>>
{
    public async Task<Result<ExtraDto>> Handle(ExtraGetQuery request, CancellationToken cancellationToken)
    {
        var res = await repository
            .GetAllWithAudit()
            .MapTo()
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (res is null)
            return Result<ExtraDto>.Failure("Ekstra bulunamadý");

        return res;
    }
}