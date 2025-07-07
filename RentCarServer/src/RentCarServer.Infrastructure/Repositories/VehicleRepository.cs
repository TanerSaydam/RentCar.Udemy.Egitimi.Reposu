using RentCarServer.Domain.Vehicles;
using RentCarServer.Infrastructure.Abstractions;
using RentCarServer.Infrastructure.Context;

namespace RentCarServer.Infrastructure.Repositories;

internal sealed class VehicleRepository : AuditableRepository<Vehicle, ApplicationDbContext>, IVehicleRepository
{
    public VehicleRepository(ApplicationDbContext context) : base(context)
    {
    }
}