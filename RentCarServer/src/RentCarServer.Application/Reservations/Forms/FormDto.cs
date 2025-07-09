using RentCarServer.Domain.Categories;
using RentCarServer.Domain.Customers;
using RentCarServer.Domain.Reservations;
using RentCarServer.Domain.Reservations.Forms.ValueObjects;
using RentCarServer.Domain.Vehicles;

namespace RentCarServer.Application.Reservations.Forms;
public sealed class FormDto
{
    public Guid ReservationId { get; set; }
    public string ReservationNumber { get; set; } = default!;
    public string ReservationStatus { get; set; } = default!;
    public DateTimeOffset PickUpDateTime { get; set; }
    public DateTimeOffset DeliveryDateTime { get; set; }
    public Guid CustomerId { get; set; }
    public int Kilometer { get; set; }
    public ReservationCustomerDto Customer { get; set; } = default!;
    public ReservationVehicleDto Vehicle { get; set; } = default!;
    public IReadOnlyCollection<string> Supplies { get; set; } = [];
    public IReadOnlyCollection<string> ImageUrls { get; set; } = [];
    public IReadOnlyCollection<Damage> Damages { get; set; } = [];
    public string Note { get; set; } = default!;
}

public static class FormExtensions
{
    public static IQueryable<FormDto> MapTo(
        this IQueryable<Reservation> entities,
        string type,
        IQueryable<Customer> customers,
        IQueryable<Vehicle> vehicles,
        IQueryable<Category> categories
        )
    {
        var res = entities
            .Join(customers, m => m.CustomerId, m => m.Id, (r, customer) => new
            {
                Entity = r,
                Customer = customer
            })
            .Join(vehicles, m => m.Entity.VehicleId, m => m.Id, (r, vehicle) => new
            {
                r.Entity,
                r.Customer,
                Vehicle = vehicle
            })
            .Select(s => new FormDto
            {
                ReservationId = s.Entity.Id,
                ReservationNumber = s.Entity.ReservationNumber.Value,
                PickUpDateTime = s.Entity.PickUpDatetime.Value,
                DeliveryDateTime = s.Entity.DeliveryDatetime.Value,
                ReservationStatus = s.Entity.Status.Value,
                CustomerId = s.Entity.CustomerId,
                Kilometer = type == "pickup" ? s.Entity.PickUpForm.Kilometer.Value : s.Entity.DeliveryForm.Kilometer.Value,
                Customer = new ReservationCustomerDto
                {
                    Email = s.Customer.Email.Value,
                    FullAddress = s.Customer.FullAddress.Value,
                    FullName = s.Customer.FullName.Value,
                    IdentityNumber = s.Customer.IdentityNumber.Value,
                    PhoneNumber = s.Customer.PhoneNumber.Value
                },
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
                Supplies = type == "pickup" ? s.Entity.PickUpForm.Supplies.Select(s => s.Value).ToArray() : s.Entity.DeliveryForm.Supplies.Select(s => s.Value).ToArray(),
                ImageUrls = type == "pickup" ? s.Entity.PickUpForm.ImageUrls.Select(s => s.Value).ToArray() : s.Entity.DeliveryForm.ImageUrls.Select(s => s.Value).ToArray(),
                Damages = type == "pickup" ? s.Entity.PickUpForm.Damages : s.Entity.DeliveryForm.Damages,
                Note = type == "pickup" ? s.Entity.PickUpForm.Note.Value : s.Entity.DeliveryForm.Note.Value
            });
        return res;
    }
}