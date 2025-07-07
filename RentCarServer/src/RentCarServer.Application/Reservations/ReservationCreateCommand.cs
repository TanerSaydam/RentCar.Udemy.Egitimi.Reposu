using FluentValidation;
using GenericRepository;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Customers;
using RentCarServer.Domain.Reservations;
using RentCarServer.Domain.Reservations.ValueObjects;
using RentCarServer.Domain.Shared;
using RentCarServer.Domain.Vehicles;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Reservations;

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
    Guid ExtraId,
    decimal ExtraPrice,
    string Note,
    CreditCartInformation CreditCartInformation,
    string CartOwner
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
        // Yeni rezervasyonun alınma ve teslim datetime’ı
        var requestedPickUp = request.PickUpDate.ToDateTime(request.PickUpTime);
        var requestedDelivery = request.DeliveryDate.ToDateTime(request.DeliveryTime);

        // Aynı araç için bu zaman aralığında çakışan rezervasyon var mı kontrol et
        var overlaps = await reservationRepository.AnyAsync(r =>
                r.VehicleId.Value == request.VehicleId &&
                (
                    requestedPickUp < r.DeliveryDate.Value.ToDateTime(r.DeliveryTime.Value).AddHours(1) &&
                    // yeni başlangıç, mevcut +1 saatten önce başlıyorsa
                    requestedDelivery > r.PickUpDate.Value.ToDateTime(r.PickUpTime.Value)
                // yeni bitiş, mevcut başlangıçtan sonra bitiyorsa
                ),
            cancellationToken: cancellationToken
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
        IdentityId extraId = new(request.ExtraId);
        Price extraPrice = new(request.ExtraPrice);
        Note note = new(request.Note);
        var last4Digits = request.CreditCartInformation.CartNumber[^4..];
        PaymentInformation paymentInformation = new(last4Digits, request.CreditCartInformation.Owner);
        Status status = Status.Pending;

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
            extraId,
            extraPrice,
            note,
            paymentInformation,
            status
        );
        #endregion

        reservationRepository.Add(reservation);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Rezervasyon başarıyla oluşturuldu";
    }
}
