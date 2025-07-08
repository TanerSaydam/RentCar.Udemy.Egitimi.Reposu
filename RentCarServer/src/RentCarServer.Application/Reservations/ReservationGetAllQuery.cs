using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Categories;
using RentCarServer.Domain.Customers;
using RentCarServer.Domain.Extras;
using RentCarServer.Domain.ProtectionPackages;
using RentCarServer.Domain.Reservations;
using RentCarServer.Domain.Vehicles;
using TS.MediatR;

namespace RentCarServer.Application.Reservations;
[Permission("reservation:view")]
public sealed record ReservationGetAllQuery : IRequest<IQueryable<ReservationDto>>;

internal sealed class ReservationGetAllQueryHandler(
    IReservationRepository reservationRepository,
    ICustomerRepository customerRepository,
    IBranchRepository brancheRepository,
    IVehicleRepository vehicleRepository,
    ICategoryRepository categoryRepository,
    IProtectionPackageRepository protectionPackageRepository,
    IExtraRepository extraRepository
    ) : IRequestHandler<ReservationGetAllQuery, IQueryable<ReservationDto>>
{
    public Task<IQueryable<ReservationDto>> Handle(ReservationGetAllQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(
            reservationRepository.GetAllWithAudit()
            .MapTo(
                customerRepository.GetAll(),
                brancheRepository.GetAll(),
                vehicleRepository.GetAll(),
                categoryRepository.GetAll(),
                protectionPackageRepository.GetAll(),
                extraRepository.GetAll())
            .AsQueryable());
}
