using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Reservations.ValueObjects;

namespace RentCarServer.Application.Reservations;

public sealed class PickUpDto
{
    public string Name { get; set; } = default!;
    public string FullAddress { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}
public sealed class CustomerDto
{
    public string FullName { get; set; } = default!;
    public string IdentityNumber { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FullAddress { get; set; } = default!;
}
public sealed class VehicleDto
{
    public string Name { get; set; } = default!;
    public string Brand { get; set; } = default!;
    public string Model { get; set; } = default!;
    public int ModelYear { get; set; } = default!;
    public string Color { get; set; } = default!;
    public string CategoryName { get; set; } = default!;
    public string FuelConsumption { get; set; } = default!;
    public int SeatCount { get; set; } = default!;
    public string TractionType { get; set; } = default!;
}
public sealed class ReservationExtraDto
{
    public Guid ExtraId { get; set; }
    public string ExtraName { get; set; } = default!;
    public decimal Price { get; set; }
}
public sealed class ReservationGetDto : EntityDto
{
    public Guid CustomerId { get; set; } = default!;
    public CustomerDto Customer { get; set; } = default!;
    public Guid PickUpLocationId { get; set; } = default!;
    public PickUpDto PickUp { get; set; } = default!;
    public DateOnly PickUpDate { get; set; } = default!;
    public TimeOnly PickUpTime { get; set; } = default!;
    public DateOnly DeliveryDate { get; set; } = default!;
    public DeliveryTime DeliveryTime { get; set; } = default!;
    public Guid VehicleId { get; set; } = default!;
    public decimal VehicleDailyPrice { get; set; } = default!;
    public VehicleDto Vehicle { get; set; } = default!;
    public Guid ProtectionPackageId { get; set; } = default!;
    public decimal ProtectionPackagePrice { get; set; } = default!;
    public string ProtectionPackageName { get; set; } = default!;

    public List<ReservationExtraDto> ReservationExtras { get; set; } = default!;
    public string Note { get; set; } = default!;
    public decimal Total { get; set; } = default!;
    public string Status { get; set; } = default!;
    public int TotalDay { get; set; } = default!;
}