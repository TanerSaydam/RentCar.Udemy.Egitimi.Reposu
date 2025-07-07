using RentCarServer.Domain.Abstractions;

namespace RentCarServer.Domain.Vehicles;

public interface IVehicleRepository : IAuditableRepository<Vehicle>
{
}