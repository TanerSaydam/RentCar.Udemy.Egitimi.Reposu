using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.Vehicles;
using TS.MediatR;

namespace RentCarServer.Application.Vehicles;

[Permission("vehicle:view")]
public sealed record VehicleGetAllQuery : IRequest<IQueryable<VehicleDto>>;

internal sealed class VehicleGetAllQueryHandler(
    IVehicleRepository vehicleRepository) : IRequestHandler<VehicleGetAllQuery, IQueryable<VehicleDto>>
{
    public Task<IQueryable<VehicleDto>> Handle(VehicleGetAllQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(vehicleRepository.GetAllWithAudit().MapTo().AsQueryable());
}