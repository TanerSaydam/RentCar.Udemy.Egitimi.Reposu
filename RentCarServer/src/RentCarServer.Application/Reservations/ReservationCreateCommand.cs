using FluentValidation;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using RentCarServer.Application.Behaviors;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Customers;
using RentCarServer.Domain.Reservations;
using RentCarServer.Domain.Reservations.ValueObjects;
using RentCarServer.Domain.Shared;
using RentCarServer.Domain.Vehicles;
using RentCarServer.Domain.Vehicles.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Reservations;
[Permission("reservation:create")]
public sealed record CreditCartInformation(
    string CartNumber,
    string Owner,
    string Expiry,
    string CCV);

public sealed record ReservationCreateCommand(
    Guid CustomerId,
    Guid? PickUpLocationId,
    DateOnly PickUpDate,
    TimeOnly PickUpTime,
    DateOnly DeliveryDate,
    TimeOnly DeliveryTime,
    Guid VehicleId,
    decimal VehicleDailyPrice,
    Guid ProtectionPackageId,
    decimal ProtectionPackagePrice,
    List<ReservationExtra> ReservationExtras,
    string Note,
    CreditCartInformation CreditCartInformation,
    decimal Total,
    int TotalDay
) : IRequest<Result<string>>;

public sealed class ReservationCreateCommandValidator : AbstractValidator<ReservationCreateCommand>
{
    public ReservationCreateCommandValidator()
    {
        RuleFor(x => x.CreditCartInformation.CartNumber)
            .NotEmpty()
            .WithMessage("Kart numarası boş bırakılamaz.");

        RuleFor(x => x.CreditCartInformation.Owner)
            .NotEmpty()
            .WithMessage("Kart sahibi adı boş bırakılamaz.");

        RuleFor(x => x.CreditCartInformation.Expiry)
           .NotEmpty()
           .WithMessage("Son kullanma tarihi boş bırakılamaz.");

        RuleFor(x => x.CreditCartInformation.CCV)
           .NotEmpty()
           .WithMessage("CCV boş bırakılamaz.");

        RuleFor(x => x.PickUpDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Teslim alma tarihi bugünden önce olamaz.");

        RuleFor(x => x.DeliveryDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Teslim etme tarihi bugünden önce olamaz.");
    }
}


internal sealed class ReservationCreateCommandHandler(
    IBranchRepository branchRepository,
    ICustomerRepository customerRepository,
    IReservationRepository reservationRepository,
    IVehicleRepository vehicleRepository,
    IClaimContext claimContext,
    IUnitOfWork unitOfWork) : IRequestHandler<ReservationCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ReservationCreateCommand request, CancellationToken cancellationToken)
    {
        var locationId = request.PickUpLocationId ?? claimContext.GetBranchId();

        #region Şube, Müşteri ve Araç Kontrolü
        var isBranchExists = await branchRepository.AnyAsync(i => i.Id == locationId, cancellationToken);
        if (!isBranchExists)
        {
            return Result<string>.Failure("Şube bulunamadı");
        }

        var isCustomerExists = await customerRepository.AnyAsync(i => i.Id == request.CustomerId, cancellationToken);
        if (!isCustomerExists)
        {
            return Result<string>.Failure("Müşteri bulunamadı");
        }

        var isVehicleExists = await vehicleRepository.AnyAsync(i => i.Id == request.VehicleId, cancellationToken);
        if (!isVehicleExists)
        {
            return Result<string>.Failure("Araç bulunamadı");
        }
        #endregion

        #region Araç Müsaitlik Kontrolü
        var requestedPickUp = request.PickUpDate.ToDateTime(request.PickUpTime);
        var requestedDelivery = request.DeliveryDate.ToDateTime(request.DeliveryTime);

        var possibleOverlaps = await reservationRepository
            .Where(r => r.VehicleId == request.VehicleId
            && (r.Status.Value == Status.Pending.Value || r.Status.Value == Status.Delivered.Value))
            .Select(s => new
            {
                Id = s.Id,
                VehicleId = s.VehicleId,
                DeliveryDate = s.DeliveryDate.Value,
                DeliveryTime = s.DeliveryTime.Value,
                PickUpDate = s.PickUpDate.Value,
                PickUpTime = s.PickUpTime.Value,
            })
            .ToListAsync(cancellationToken);

        var overlaps = possibleOverlaps.Any(r =>
            requestedPickUp < r.DeliveryDate.ToDateTime(r.DeliveryTime).AddHours(1) &&
            requestedDelivery > r.PickUpDate.ToDateTime(r.PickUpTime)
        );

        if (overlaps)
        {
            return Result<string>.Failure("Seçilen araç, belirtilen tarih ve saat aralığında müsait değil.");
        }
        #endregion

        #region Ödeme İşlemi
        // ödeme işlemi yapıp başarılı ise ona göre devam etmeliyiz
        #endregion

        #region Reservation Objesinin Oluşturulması
        IdentityId customerId = new(request.CustomerId);
        IdentityId pickUpLocationId = new(locationId);
        PickUpDate pickUpDate = new(request.PickUpDate);
        PickUpTime pickUpTime = new(request.PickUpTime);
        DeliveryDate deliveryDate = new(request.DeliveryDate);
        DeliveryTime deliveryTime = new(request.DeliveryTime);
        IdentityId vehicleId = new(request.VehicleId);
        Price vehicleDailyPrice = new(request.VehicleDailyPrice);
        IdentityId protectionPackageId = new(request.ProtectionPackageId);
        Price protectionPackagePrice = new(request.ProtectionPackagePrice);
        IEnumerable<ReservationExtra> reservationExtras = request.ReservationExtras.Select(s => new ReservationExtra(s.ExtraId, s.Price));
        Note note = new(request.Note);
        var last4Digits = request.CreditCartInformation.CartNumber[^4..];
        PaymentInformation paymentInformation = new(last4Digits, request.CreditCartInformation.Owner);
        Status status = Status.Pending;
        Total total = new(request.Total);
        TotalDay totalDay = new(request.TotalDay);
        ReservationHistory history = new("Rezervayon Oluşturuldu", "Online olarak rezervasyon oluşturuldu", DateTimeOffset.Now);

        Form? pickUpForm = await reservationRepository
            .Where(p => p.VehicleId == vehicleId)
            .OrderByDescending(p => p.CreatedAt)
            .Select(s => s.PickUpForm)
            .FirstOrDefaultAsync(cancellationToken);

        Form deliveryForm;

        if (pickUpForm is null)
        {
            var kilometer = await vehicleRepository
                .Where(p => p.Id == request.VehicleId)
                .Select(s => s.Kilometer)
                .FirstAsync(cancellationToken);
            List<Supplies> supplies = new();
            List<Damage> damages = new();
            List<ImageUrl> imageUrls = new();
            Note formNote = new(string.Empty);
            pickUpForm = new(
                kilometer,
                supplies,
                imageUrls,
                damages,
                formNote);
        }
        else
        {
            if (pickUpForm.Kilometer.Value == 0)
            {
                var kilometer = await vehicleRepository
                    .Where(p => p.Id == request.VehicleId)
                    .Select(s => s.Kilometer)
                    .FirstAsync(cancellationToken);
                pickUpForm.SetKilometer(kilometer);
            }
        }

        deliveryForm = new(
            pickUpForm.Kilometer,
            pickUpForm.Supplies.ToList(),
            pickUpForm.ImageUrls.ToList(),
            pickUpForm.Damages.ToList(),
            pickUpForm.Note);

        Reservation reservation = Reservation.Create(
            customerId,
            pickUpLocationId,
            pickUpDate,
            pickUpTime,
            deliveryDate,
            deliveryTime,
            vehicleId,
            vehicleDailyPrice,
            protectionPackageId,
            protectionPackagePrice,
            reservationExtras,
            note,
            paymentInformation,
            status,
            total,
            totalDay,
            history,
            pickUpForm,
            deliveryForm
        );

        ReservationHistory history2 = new("Ödeme Alındı", "Reservasyonun ödemesi başarıyla alındı", DateTimeOffset.Now);
        reservation.SetHistory(history2);
        #endregion

        reservationRepository.Add(reservation);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Rezervasyon başarıyla oluşturuldu";
    }
}
