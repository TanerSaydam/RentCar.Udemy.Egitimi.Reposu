using GenericRepository;
using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.Roles;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Roles;
[Permission("role:update_permissions")]
public sealed record RoleUpdatePermissionsCommand(
    Guid RoleId,
    List<string> Permissions) : IRequest<Result<string>>;

internal sealed class RoleUpdatePermissionsCommandHandler(
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RoleUpdatePermissionsCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleUpdatePermissionsCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.FirstOrDefaultAsync(p => p.Id == request.RoleId, cancellationToken);

        if (role is null)
        {
            return Result<string>.Failure("Rol bulunamadı");
        }

        List<Permission> permissions = request.Permissions.Select(s => new Permission(s)).ToList();
        role.SetPermissions(permissions);
        roleRepository.Update(role);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "İşlem başarıyla tamamlandı";
    }
}
