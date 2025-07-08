using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Reservations.ValueObjects;
using RentCarServer.Domain.Shared;

namespace RentCarServer.Domain.Reservations;
public sealed class Reservation : Entity, IAggregate
{
    private readonly List<ReservationExtra> _reservationExtras = new();
    private Reservation() { }
    private Reservation(
        IdentityId customerId,
        IdentityId pickUpLocationId,
        PickUpDate pickUpDate,
        PickUpTime pickUpTime,
        DeliveryDate deliveryDate,
        DeliveryTime deliveryTime,
        IdentityId vehicleId,
        Price vehicleDailyPrice,
        IdentityId protectionPackageId,
        Price protectionPackagePrice,
        IEnumerable<ReservationExtra> reservationExtras,
        Note note,
        PaymentInformation paymentInformation,
        Status status,
        Total total,
        TotalDay totalDay)
    {
        SetCustomerId(customerId);
        SetPickUpLocationId(pickUpLocationId);
        SetPickUpDate(pickUpDate);
        SetPickUpTime(pickUpTime);
        SetDeliveryDate(deliveryDate);
        SetDeliveryTime(deliveryTime);
        SetVehicleId(vehicleId);
        SetVehicleDailyPrice(vehicleDailyPrice);
        SetProtectionPackageId(protectionPackageId);
        SetProtectionPackagePrice(protectionPackagePrice);
        SetReservationExtras(reservationExtras);
        SetNote(note);
        SetPaymentInformation(paymentInformation);
        SetStatus(status);
        SetTotalDay(totalDay);
        SetTotal(total);
        SetPickupDateTime();
        SetDeliveryDateTime();
        SetReservationNumber();
    }
    public ReservationNumber ReservationNumber { get; private set; } = default!;
    public IdentityId CustomerId { get; private set; } = default!;
    public IdentityId PickUpLocationId { get; private set; } = default!;
    public PickUpDate PickUpDate { get; private set; } = default!;
    public PickUpTime PickUpTime { get; private set; } = default!;
    public PickUpDatetime PickUpDatetime { get; private set; } = default!;
    public DeliveryDate DeliveryDate { get; private set; } = default!;
    public DeliveryTime DeliveryTime { get; private set; } = default!;
    public DeliveryDatetime DeliveryDatetime { get; private set; } = default!;
    public TotalDay TotalDay { get; private set; } = default!;
    public IdentityId VehicleId { get; private set; } = default!;
    public Price VehicleDailyPrice { get; private set; } = default!;
    public IdentityId ProtectionPackageId { get; private set; } = default!;
    public Price ProtectionPackagePrice { get; private set; } = default!;
    public IReadOnlyCollection<ReservationExtra> ReservationExtras => _reservationExtras;
    public Note Note { get; private set; } = default!;
    public PaymentInformation PaymentInformation { get; private set; } = default!;
    public Status Status { get; private set; } = default!;
    public Total Total { get; private set; } = default!;

    public static Reservation Create(
        IdentityId customerId,
        IdentityId pickUpLocationId,
        PickUpDate pickUpDate,
        PickUpTime pickUpTime,
        DeliveryDate deliveryDate,
        DeliveryTime deliveryTime,
        IdentityId vehicleId,
        Price vehicleDailyPrice,
        IdentityId protectionPackageId,
        Price protectionPackagePrice,
        IEnumerable<ReservationExtra> reservationExtras,
        Note note,
        PaymentInformation paymentInformation,
        Status status,
        Total total,
        TotalDay totalDay)
    {
        var reservation = new Reservation(
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
            totalDay
        );

        return reservation;
    }

    #region Behaviors
    public void SetCustomerId(IdentityId customerId)
    {
        CustomerId = customerId;
    }

    public void SetPickUpLocationId(IdentityId pickUpLocationId)
    {
        PickUpLocationId = pickUpLocationId;
    }

    public void SetPickUpDate(PickUpDate pickUpDate)
    {
        PickUpDate = pickUpDate;
    }

    public void SetPickUpTime(PickUpTime pickUpTime)
    {
        PickUpTime = pickUpTime;
    }

    public void SetDeliveryDate(DeliveryDate deliveryDate)
    {
        DeliveryDate = deliveryDate;
    }

    public void SetDeliveryTime(DeliveryTime deliveryTime)
    {
        DeliveryTime = deliveryTime;
    }
    public void SetTotalDay(TotalDay totalDay)
    {
        TotalDay = totalDay;
    }

    public void SetVehicleId(IdentityId vehicleId)
    {
        VehicleId = vehicleId;
    }

    public void SetVehicleDailyPrice(Price vehicleDailyPrice)
    {
        VehicleDailyPrice = vehicleDailyPrice;
    }

    public void SetProtectionPackageId(IdentityId protectionPackageId)
    {
        ProtectionPackageId = protectionPackageId;
    }

    public void SetProtectionPackagePrice(Price protectionPackagePrice)
    {
        ProtectionPackagePrice = protectionPackagePrice;
    }

    public void SetReservationExtras(IEnumerable<ReservationExtra> reservationExtras)
    {
        _reservationExtras.Clear();
        _reservationExtras.AddRange(reservationExtras);
    }

    public void SetNote(Note note)
    {
        Note = note;
    }

    public void SetPaymentInformation(PaymentInformation paymentInformation)
    {
        PaymentInformation = paymentInformation;
    }

    public void SetStatus(Status status)
    {
        Status = status;
    }

    public void SetTotal(Total total)
    {
        Total = total;
    }

    public void SetPickupDateTime()
    {
        var date = new DateTime(PickUpDate.Value, PickUpTime.Value);
        PickUpDatetime = new(new DateTimeOffset(date));
    }

    public void SetDeliveryDateTime()
    {
        var date = new DateTime(DeliveryDate.Value, DeliveryTime.Value);
        DeliveryDatetime = new(new DateTimeOffset(date));
    }

    public void SetReservationNumber()
    {
        var date = DateTime.Now;
        Random random = new();
        var num = string.Concat(Enumerable.Range(0, 8).Select(_ => random.Next(10)));
        string number = "RSV-" + date.Year + "-" + num;
        ReservationNumber = new(number);
    }
    #endregion
}