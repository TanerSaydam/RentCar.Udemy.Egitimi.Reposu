using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.Extras;
using TS.MediatR;

namespace RentCarServer.Application.Extras;

public sealed record ExtraGetAllQuery : IRequest<IQueryable<ExtraDto>>;
[Permission("extra:view")]
internal sealed class ExtraGetAllQueryHandler(
    IExtraRepository repository) : IRequestHandler<ExtraGetAllQuery, IQueryable<ExtraDto>>
{
    public Task<IQueryable<ExtraDto>> Handle(ExtraGetAllQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(repository.GetAllWithAudit().MapTo().AsQueryable());
}