using FluentValidation;
using GenericRepository;
using RentCarServer.Domain.Branchs.ValueObjects;
using RentCarServer.Domain.Roles;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Roles;
public sealed record RoleCreateCommand(
    string Name,
    bool IsActive) : IRequest<Result<string>>;


public sealed class RoleCreateCommandValidator : AbstractValidator<RoleCreateCommand>
{
    public RoleCreateCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Geçerli bir rol adı girin");
    }
}

internal sealed class RoleCreateCommandHandler(
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RoleCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
    {
        var nameExists = await roleRepository.AnyAsync(p => p.Name.Value == request.Name, cancellationToken);

        if (nameExists)
        {
            return Result<string>.Failure("Rol adı daha önce tanımlanmış");
        }

        Name name = new(request.Name);
        Role role = new(name);
        roleRepository.Add(role);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Role başarıyla kaydedildi";
    }
}