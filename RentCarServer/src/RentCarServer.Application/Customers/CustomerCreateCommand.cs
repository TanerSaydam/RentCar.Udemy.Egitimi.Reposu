using FluentValidation;
using GenericRepository;
using RentCarServer.Domain.Customers;
using RentCarServer.Domain.Shared;
using RentCarServer.Domain.Users.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Customers;

public sealed record CustomerCreateCommand(
    string FirstName,
    string LastName,
    string IdentityNumber,
    DateOnly DateOfBirth,
    string PhoneNumber,
    string Email,
    DateOnly DrivingLicenseIssuanceDate,
    string FullAddress,
    bool IsActive
) : IRequest<Result<string>>;

public sealed class CustomerCreateCommandValidator : AbstractValidator<CustomerCreateCommand>
{
    public CustomerCreateCommandValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().WithMessage("Ad alan� bo� olamaz.");
        RuleFor(p => p.LastName).NotEmpty().WithMessage("Soyad alan� bo� olamaz.");
        RuleFor(p => p.IdentityNumber).NotEmpty().WithMessage("TC kimlik numaras� bo� olamaz.");
        RuleFor(p => p.Email).NotEmpty().WithMessage("E-posta adresi bo� olamaz.")
                             .EmailAddress().WithMessage("Ge�erli bir e-posta adresi giriniz.");
        RuleFor(p => p.PhoneNumber).NotEmpty().WithMessage("Telefon numaras� bo� olamaz.");
        RuleFor(p => p.FullAddress).NotEmpty().WithMessage("Adres alan� bo� olamaz.");
    }
}

internal sealed class CustomerCreateCommandHandler(
    ICustomerRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<CustomerCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CustomerCreateCommand request, CancellationToken cancellationToken)
    {
        bool exists = await repository.AnyAsync(x => x.IdentityNumber.Value == request.IdentityNumber, cancellationToken);
        if (exists)
            return Result<string>.Failure("Bu TC kimlik numaras� ile kay�tl� m��teri var.");

        FirstName firstName = new(request.FirstName);
        LastName lastName = new(request.LastName);
        IdentityNumber identityNumber = new(request.IdentityNumber);
        DateOfBirth dateOfBirth = new(request.DateOfBirth);
        PhoneNumber phoneNumber = new(request.PhoneNumber);
        Email email = new(request.Email);
        DrivingLicenseIssuanceDate drivingLicenseIssuanceDate = new(request.DrivingLicenseIssuanceDate);
        FullAddress fullAddress = new(request.FullAddress);
        Password password = new("123");

        Customer customer = new(
            firstName,
            lastName,
            identityNumber,
            dateOfBirth,
            phoneNumber,
            email,
            drivingLicenseIssuanceDate,
            fullAddress,
            password,
            request.IsActive
        );

        repository.Add(customer);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "M��teri ba�ar�yla kaydedildi";
    }
}