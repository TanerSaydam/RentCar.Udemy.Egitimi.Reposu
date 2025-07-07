using RentCarServer.Domain.Abstractions;

namespace RentCarServer.Application.Reservations;
public sealed class ReservationGetDto : EntityDto
{
    public Guid CustomerId { get; set; } = default!;
    public string CustomerFullName { get; set; } = default!;
    public string IdentityNumber { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FullAddress { get; set; } = default!;
    public DateTime ReservationDate { get; set; } = default!;
    public DateTime DeliveryDate { get; set; } = default!;
    public string Status { get; set; } = default!;
    public int TotalDay { get; set; } = default!;
    public Guid VehicleId { get; set; } = default!;
    public string VehicleName { get; set; } = default!;
    public string VehicleBrand { get; set; } = default!;
    public string VehicleModel { get; set; } = default!;
    public int VehicleModelYear { get; set; } = default!;
    public string VehicleColor { get; set; } = default!;
    public string VehicleCategoryName { get; set; } = default!;
    public string FuelConsumption { get; set; } = default!;
    public int SeatCount { get; set; } = default!;
    public string TractionType { get; set; } = default!;
    public Guid PickupLocationId { get; set; } = default!;
    public string PickupLocationName { get; set; } = default!;
    public string PickupLocationFullAddress { get; set; } = default!;
    public string PickupLocationPhoneNumber { get; set; } = default!;
    public Guid ProtectionPackageId { get; set; } = default!;
    public string ProtectionPackageName { get; set; } = default!;
    public decimal ProtectionPackagePrice { get; set; } = default!;
    public List<ReservationExtraDto> ReservationExtras { get; set; } = default!;
}


public sealed class ReservationExtraDto
{
    public Guid ExtraId { get; set; }
    public string ExtraName { get; set; } = default!;
    public decimal Price { get; set; }
}