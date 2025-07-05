using Microsoft.EntityFrameworkCore;
using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.Roles;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Roles;
[Permission("role:view")]
public sealed record RoleGetQuery(
    Guid Id) : IRequest<Result<RoleDto>>;

internal sealed class RoleGetQueryHandler(
    IRoleRepository roleRepository) : IRequestHandler<RoleGetQuery, Result<RoleDto>>
{
    public async Task<Result<RoleDto>> Handle(RoleGetQuery request, CancellationToken cancellationToken)
    {
        var res = await roleRepository
            .GetAllWithAudit()
            .MapToGet()
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (res is null)
        {
            return Result<RoleDto>.Failure("Rol bulunamadı");
        }

        return res;
    }
}