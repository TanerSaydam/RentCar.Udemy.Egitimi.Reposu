using RentCarServer.Domain.Roles;
using TS.MediatR;

namespace RentCarServer.Application.Roles;
public sealed record RoleGetAllQuery : IRequest<IQueryable<RoleDto>>;

internal sealed class RoleGetAllQueryHandler(
    IRoleRepository roleRepository) : IRequestHandler<RoleGetAllQuery, IQueryable<RoleDto>>
{
    public Task<IQueryable<RoleDto>> Handle(RoleGetAllQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(roleRepository.GetAllWithAudit().MapTo().AsQueryable());
}