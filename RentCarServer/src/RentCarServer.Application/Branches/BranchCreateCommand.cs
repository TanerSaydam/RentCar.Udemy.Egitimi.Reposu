using FluentValidation;
using GenericRepository;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Branchs;
using RentCarServer.Domain.Branchs.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Branches;
public sealed record BranchCreateCommand(
    string Name,
    Address Address,
    bool IsActive) : IRequest<Result<string>>;

public sealed class BranchCreateCommandValidator : AbstractValidator<BranchCreateCommand>
{
    public BranchCreateCommandValidator()
    {
        RuleFor(i => i.Name).NotEmpty().WithMessage("Geçerli bir şube adı girin");
        RuleFor(i => i.Address.City).NotEmpty().WithMessage("Geçerli bir şehir seçin");
        RuleFor(i => i.Address.District).NotEmpty().WithMessage("Geçerli bir ilçe seçin");
        RuleFor(i => i.Address.FullAddress).NotEmpty().WithMessage("Geçerli bir tam adres girin");
        RuleFor(i => i.Address.PhoneNumber1).NotEmpty().WithMessage("Geçerli bir telefon numarası girin");
    }
}

internal sealed class BranchCreateCommandHandler(
    IBranchRepository branchRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<BranchCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(BranchCreateCommand request, CancellationToken cancellationToken)
    {
        var nameIsExists = await branchRepository.AnyAsync(p => p.Name.Value == request.Name, cancellationToken);
        if (nameIsExists)
        {
            return Result<string>.Failure("Bu şube adı daha önce kullanılmış");
        }

        Name name = new(request.Name);
        Address address = request.Address;
        Branch branch = new(name, address, request.IsActive);
        branchRepository.Add(branch);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Şube başarılı şekilde kaydedili";
    }
}