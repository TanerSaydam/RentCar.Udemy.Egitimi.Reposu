using Microsoft.EntityFrameworkCore;
using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.ProtectionPackages;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.ProtectionPackages;
[Permission("protection_package:view")]
public sealed record ProtectionPackageGetQuery(Guid Id) : IRequest<Result<ProtectionPackageDto>>;

internal sealed class ProtectionPackageGetQueryHandler(
    IProtectionPackageRepository repository) : IRequestHandler<ProtectionPackageGetQuery, Result<ProtectionPackageDto>>
{
    public async Task<Result<ProtectionPackageDto>> Handle(ProtectionPackageGetQuery request, CancellationToken cancellationToken)
    {
        var res = await repository
            .GetAllWithAudit()
            .MapTo()
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (res is null)
            return Result<ProtectionPackageDto>.Failure("Güvence paketi bulunamadý");

        return res;
    }
}