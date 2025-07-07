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
public sealed record ReservationUpdateCommand(
    Guid Id,
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
    decimal Total
) : IRequest<Result<string>>;

public sealed class ReservationUpdateCommandValidator : AbstractValidator<ReservationUpdateCommand>
{
    public ReservationUpdateCommandValidator()
    {
        RuleFor(x => x.PickUpDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Teslim alma tarihi bugünden önce olamaz.");

        RuleFor(x => x.DeliveryDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Teslim etme tarihi bugünden önce olamaz.");
    }
}

internal sealed class ReservationUpdateCommandHandler(
    IBranchRepository branchRepository,
    ICustomerRepository customerRepository,
    IReservationRepository reservationRepository,
    IVehicleRepository vehicleRepository,
    IClaimContext claimContext,
    IUnitOfWork unitOfWork) : IRequestHandler<ReservationUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ReservationUpdateCommand request, CancellationToken cancellationToken)
    {
        Reservation? reservation = await reservationRepository.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (reservation is null)
        {
            return Result<string>.Failure("Rezervasyon bulunamadı");
        }

        if (reservation.Status == Status.Completed || reservation.Status == Status.Canceled)
        {
            return Result<string>.Failure("Bu rezervasyon değiştirilemez");
        }

        var locationId = request.PickUpLocationId ?? claimContext.GetBranchId();

        #region Şube, Müşteri ve Araç Kontrolü
        if (reservation.PickUpLocationId.Value != locationId)
        {
            var isBranchExists = await branchRepository.AnyAsync(i => i.Id == locationId, cancellationToken);
            if (!isBranchExists)
            {
                return Result<string>.Failure("Şube bulunamadı");
            }
        }

        if (reservation.CustomerId != request.CustomerId)
        {
            var isCustomerExists = await customerRepository.AnyAsync(i => i.Id == request.CustomerId, cancellationToken);
            if (!isCustomerExists)
            {
                return Result<string>.Failure("Müşteri bulunamadı");
            }
        }

        if (reservation.VehicleId != request.VehicleId)
        {
            var isVehicleExists = await vehicleRepository.AnyAsync(i => i.Id == request.VehicleId, cancellationToken);
            if (!isVehicleExists)
            {
                return Result<string>.Failure("Araç bulunamadı");
            }
        }
        #endregion

        #region Araç Müsaitlik Kontrolü
        if (reservation.PickUpDate.Value != request.PickUpDate
            || reservation.PickUpTime.Value != request.PickUpTime
            | reservation.DeliveryDate.Value != request.DeliveryDate
            || reservation.DeliveryTime.Value != request.DeliveryTime
            )
        {
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
        }
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
        Total total = new(request.Total);

        reservation.SetCustomerId(customerId);
        reservation.SetPickUpLocationId(pickUpLocationId);
        reservation.SetPickUpDate(pickUpDate);
        reservation.SetPickUpTime(pickUpTime);
        reservation.SetDeliveryDate(deliveryDate);
        reservation.SetDeliveryTime(deliveryTime);
        reservation.SetVehicleId(vehicleId);
        reservation.SetVehicleDailyPrice(vehicleDailyPrice);
        reservation.SetProtectionPackageId(protectionPackageId);
        reservation.SetProtectionPackagePrice(protectionPackagePrice);
        reservation.SetReservationExtras(reservationExtras);
        reservation.SetNote(note);
        reservation.SetTotal(total);

        #endregion

        reservationRepository.Update(reservation);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Rezervasyon başarıyla güncellendi";
    }
}
