using FluentValidation;
using GenericRepository;
using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.ProtectionPackages;
using RentCarServer.Domain.ProtectionPackages.ValueObjects;
using RentCarServer.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.ProtectionPackages;
[Permission("protection_package:create")]
public sealed record ProtectionPackageCreateCommand(
    string Name,
    decimal Price,
    bool IsRecommended,
    int OrderNumber,
    List<string> Coverages,
    bool IsActive) : IRequest<Result<string>>;

public sealed class ProtectionPackageCreateCommandValidator : AbstractValidator<ProtectionPackageCreateCommand>
{
    public ProtectionPackageCreateCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Ge�erli bir paket ad� girin");
        RuleFor(p => p.Price).GreaterThan(-1).WithMessage("Fiyat pozitif olmal�");
    }
}

internal sealed class ProtectionPackageCreateCommandHandler(
    IProtectionPackageRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<ProtectionPackageCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ProtectionPackageCreateCommand request, CancellationToken cancellationToken)
    {
        var nameExists = await repository.AnyAsync(p => p.Name.Value == request.Name, cancellationToken);
        if (nameExists)
            return Result<string>.Failure("Paket ad� daha �nce tan�mlanm��");

        Name name = new(request.Name);
        Price price = new(request.Price);
        IsRecommended isRecommended = new(request.IsRecommended);
        OrderNumber orderNumber = new(request.OrderNumber);
        List<ProtectionCoverage> coverages = request.Coverages.Select(c => new ProtectionCoverage(c)).ToList();

        ProtectionPackage package = new(
            name,
            price,
            isRecommended,
            orderNumber,
            coverages,
            request.IsActive);

        repository.Add(package);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "G�vence paketi ba�ar�yla kaydedildi";
    }
}