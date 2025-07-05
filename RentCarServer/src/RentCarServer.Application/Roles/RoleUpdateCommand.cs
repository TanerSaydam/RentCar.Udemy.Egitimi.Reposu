using FluentValidation;
using GenericRepository;
using RentCarServer.Domain.Branchs.ValueObjects;
using RentCarServer.Domain.Roles;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Roles;
public sealed record RoleUpdateCommand(
    Guid Id,
    string Name,
    bool IsActive) : IRequest<Result<string>>;

public sealed class RoleUpdateCommandValidator : AbstractValidator<RoleUpdateCommand>
{
    public RoleUpdateCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Geçerli bir rol adı girin");
    }
}

internal sealed class RoleUpdateCommandHandler(
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RoleUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (role is null)
        {
            return Result<string>.Failure("Rol bulunamadı");
        }

        Name name = new(request.Name);
        role.SetName(name);
        role.SetStatus(request.IsActive);
        roleRepository.Update(role);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Rol başarıyla güncellendi";
    }
}