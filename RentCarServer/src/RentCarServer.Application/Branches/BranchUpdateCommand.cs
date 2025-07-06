using FluentValidation;
using GenericRepository;
using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Branches;
[Permission("branch:edit")]
public sealed record BranchUpdateCommand(
    Guid Id,
    string Name,
    Address Address,
    Contact Contact,
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
        RuleFor(i => i.Contact.PhoneNumber1).NotEmpty().WithMessage("Geçerli bir telefon numarası girin");
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
        Contact contact = request.Contact;

        branch.SetName(name);
        branch.SetAddress(address);
        branch.SetContact(contact);
        branch.SetStatus(request.IsActive);
        branchRepository.Update(branch);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Şube bilgisi başarıyla güncellendi";
    }
}