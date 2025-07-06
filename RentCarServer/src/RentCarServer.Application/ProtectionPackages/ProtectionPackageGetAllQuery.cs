using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.ProtectionPackages;
using TS.MediatR;

namespace RentCarServer.Application.ProtectionPackages;

public sealed record ProtectionPackageGetAllQuery : IRequest<IQueryable<ProtectionPackageDto>>;
[Permission("protection_package:view")]
internal sealed class ProtectionPackageGetAllQueryHandler(
    IProtectionPackageRepository repository) : IRequestHandler<ProtectionPackageGetAllQuery, IQueryable<ProtectionPackageDto>>
{
    public Task<IQueryable<ProtectionPackageDto>> Handle(ProtectionPackageGetAllQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(repository.GetAllWithAudit().MapTo().AsQueryable());
}