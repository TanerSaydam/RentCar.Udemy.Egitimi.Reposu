using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Categories;
using RentCarServer.Domain.Customers;
using RentCarServer.Domain.Extras;
using RentCarServer.Domain.ProtectionPackages;
using RentCarServer.Domain.Reservations;
using RentCarServer.Domain.Reservations.ValueObjects;
using RentCarServer.Domain.Vehicles;

namespace RentCarServer.Application.Reservations;

public sealed class ReservationPickUpDto
{
    public string Name { get; set; } = default!;
    public string FullAddress { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}
public sealed class ReservationCustomerDto
{
    public string FullName { get; set; } = default!;
    public string IdentityNumber { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FullAddress { get; set; } = default!;
}
public sealed class ReservationVehicleDto
{
    public Guid Id { get; set; } = default!;
    public string Brand { get; set; } = default!;
    public string Model { get; set; } = default!;
    public int ModelYear { get; set; } = default!;
    public string Color { get; set; } = default!;
    public string CategoryName { get; set; } = default!;
    public decimal FuelConsumption { get; set; } = default!;
    public int SeatCount { get; set; } = default!;
    public string TractionType { get; set; } = default!;
    public int Kilometer { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public string Plate { get; set; } = default!;
}
public sealed class ReservationExtraDto
{
    public Guid ExtraId { get; set; }
    public string ExtraName { get; set; } = default!;
    public decimal Price { get; set; }
}
public sealed class ReservationDto : EntityDto
{
    public string ReservationNumber { get; set; } = default!;
    public Guid CustomerId { get; set; } = default!;
    public ReservationCustomerDto Customer { get; set; } = default!;
    public Guid PickUpLocationId { get; set; } = default!;
    public ReservationPickUpDto PickUp { get; set; } = default!;
    public DateOnly PickUpDate { get; set; } = default!;
    public TimeOnly PickUpTime { get; set; } = default!;
    public DateTimeOffset PickUpDateTime { get; set; } = default!;
    public DateOnly DeliveryDate { get; set; } = default!;
    public TimeOnly DeliveryTime { get; set; } = default!;
    public DateTimeOffset DeliveryDateTime { get; set; } = default!;
    public Guid VehicleId { get; set; } = default!;
    public decimal VehicleDailyPrice { get; set; } = default!;
    public ReservationVehicleDto Vehicle { get; set; } = default!;
    public Guid ProtectionPackageId { get; set; } = default!;
    public decimal ProtectionPackagePrice { get; set; } = default!;
    public string ProtectionPackageName { get; set; } = default!;
    public List<ReservationExtraDto> ReservationExtras { get; set; } = default!;
    public string Note { get; set; } = default!;
    public decimal Total { get; set; } = default!;
    public string Status { get; set; } = default!;
    public int TotalDay { get; set; } = default!;
    public PaymentInformation PaymentInformation { get; set; } = default!;
}

public static class ReservationExtensions
{
    public static IQueryable<ReservationDto> MapTo(
        this IQueryable<EntityWithAuditDto<Reservation>> entities,
             IQueryable<Customer> customers,
             IQueryable<Branch> branches,
             IQueryable<Vehicle> vehicles,
             IQueryable<Category> categories,
             IQueryable<ProtectionPackage> protectionPackages,
             IQueryable<Extra> extras
       )
    {
        var res = entities
            .Join(customers, m => m.Entity.CustomerId, m => m.Id, (r, customer) => new
            {
                r.Entity,
                r.CreatedUser,
                r.UpdatedUser,
                Customer = customer
            })
            .Join(branches, m => m.Entity.PickUpLocationId, m => m.Id, (r, branch) => new
            {
                r.Entity,
                r.CreatedUser,
                r.UpdatedUser,
                r.Customer,
                Branch = branch
            })
            .Join(protectionPackages, m => m.Entity.ProtectionPackageId, m => m.Id, (r, protectionPackage) => new
            {
                r.Entity,
                r.CreatedUser,
                r.UpdatedUser,
                r.Customer,
                r.Branch,
                ProtectionPackage = protectionPackage
            })
            .Join(vehicles, m => m.Entity.VehicleId, m => m.Id, (r, vehicle) => new
            {
                r.Entity,
                r.CreatedUser,
                r.UpdatedUser,
                r.Customer,
                r.Branch,
                r.ProtectionPackage,
                Vehicle = vehicle
            })
            .Select(s => new ReservationDto
            {
                Id = s.Entity.Id,
                ReservationNumber = s.Entity.ReservationNumber.Value,
                CustomerId = s.Entity.CustomerId,
                Customer = new ReservationCustomerDto
                {
                    Email = s.Customer.Email.Value,
                    FullAddress = s.Customer.FullAddress.Value,
                    FullName = s.Customer.FullName.Value,
                    IdentityNumber = s.Customer.IdentityNumber.Value,
                    PhoneNumber = s.Customer.PhoneNumber.Value
                },
                PickUpLocationId = s.Entity.PickUpLocationId,
                PickUp = new ReservationPickUpDto
                {
                    Name = s.Branch.Name.Value,
                    FullAddress = s.Branch.Address.FullAddress,
                    PhoneNumber = s.Branch.Contact.PhoneNumber1
                },
                PickUpDate = s.Entity.PickUpDate.Value,
                PickUpTime = s.Entity.PickUpTime.Value,
                PickUpDateTime = s.Entity.PickUpDatetime.Value,
                DeliveryDate = s.Entity.DeliveryDate.Value,
                DeliveryTime = s.Entity.DeliveryTime.Value,
                DeliveryDateTime = s.Entity.DeliveryDatetime.Value,
                VehicleId = s.Entity.VehicleId.Value,
                VehicleDailyPrice = s.Entity.VehicleDailyPrice.Value,
                Vehicle = new ReservationVehicleDto
                {
                    Id = s.Vehicle.Id,
                    Brand = s.Vehicle.Brand.Value,
                    Model = s.Vehicle.Model.Value,
                    ModelYear = s.Vehicle.ModelYear.Value,
                    CategoryName = categories.First(i => i.Id == s.Vehicle.CategoryId.Value).Name.Value,
                    Color = s.Vehicle.Color.Value,
                    FuelConsumption = s.Vehicle.FuelConsumption.Value,
                    SeatCount = s.Vehicle.SeatCount.Value,
                    TractionType = s.Vehicle.TractionType.Value,
                    Kilometer = s.Vehicle.Kilometer.Value,
                    ImageUrl = s.Vehicle.ImageUrl.Value,
                    Plate = s.Vehicle.Plate.Value
                },
                ProtectionPackageId = s.Entity.ProtectionPackageId.Value,
                ProtectionPackagePrice = s.Entity.ProtectionPackagePrice.Value,
                ProtectionPackageName = s.ProtectionPackage.Name.Value,
                ReservationExtras = s.Entity.ReservationExtras.Join(extras, m => m.ExtraId, m => m.Id, (re, extra) => new ReservationExtraDto
                {
                    ExtraId = re.ExtraId,
                    ExtraName = extra.Name.Value,
                    Price = re.Price
                }).ToList(),
                Note = s.Entity.Note.Value,
                Total = s.Entity.Total.Value,
                TotalDay = s.Entity.TotalDay.Value,
                Status = s.Entity.Status.Value,
                PaymentInformation = s.Entity.PaymentInformation,
                IsActive = s.Entity.IsActive,
                CreatedAt = s.Entity.CreatedAt,
                CreatedBy = s.Entity.CreatedBy.Value,
                CreatedFullName = s.CreatedUser.FullName.Value,
                UpdatedAt = s.Entity.UpdatedAt,
                UpdatedBy = s.Entity.UpdatedBy != null ? s.Entity.UpdatedBy.Value : null,
                UpdatedFullName = s.UpdatedUser != null ? s.UpdatedUser.FullName.Value : null,
            });
        return res;
    }
}