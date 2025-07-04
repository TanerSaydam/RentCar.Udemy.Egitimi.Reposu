using FluentValidation;
using GenericRepository;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Branchs.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Branches;
public sealed record BranchUpdateCommand(
    Guid Id,
    string Name,
    Address Address,
    bool IsActive
    ) : IRequest<Result<string>>;

public sealed class BranchUpdateCommandValidator : AbstractValidator<BranchUpdateCommand>
{
    public BranchUpdateCommandValidator()
    {
        RuleFor(i => i.Name).NotEmpty().WithMessage("Geçerli bir şube adı girin");
        RuleFor(i => i.Address.City).NotEmpty().WithMessage("Geçerli bir şehir seçin");
        RuleFor(i => i.Address.District).NotEmpty().WithMessage("Geçerli bir ilçe seçin");
        RuleFor(i => i.Address.FullAddress).NotEmpty().WithMessage("Geçerli bir tam adres girin");
        RuleFor(i => i.Address.PhoneNumber1).NotEmpty().WithMessage("Geçerli bir telefon numarası girin");
    }
}

internal sealed class BranchUpdateCommandHandler(
    IBranchRepository branchRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<BranchUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(BranchUpdateCommand request, CancellationToken cancellationToken)
    {
        var branch = await branchRepository.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        if (branch is null)
        {
            return Result<string>.Failure("Şube bulunamadı");
        }

        Name name = new(request.Name);
        Address address = request.Address;

        branch.SetName(name);
        branch.SetAddress(address);
        branch.SetStatus(request.IsActive);
        branchRepository.Update(branch);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Şube bilgisi başarıyla güncellendi";
    }
}