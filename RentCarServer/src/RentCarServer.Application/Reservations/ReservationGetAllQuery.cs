using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Categories;
using RentCarServer.Domain.Customers;
using RentCarServer.Domain.Extras;
using RentCarServer.Domain.ProtectionPackages;
using RentCarServer.Domain.Reservations;
using RentCarServer.Domain.Vehicles;
using TS.MediatR;

namespace RentCarServer.Application.Reservations;
public sealed record ReservationGetAllQuery : IRequest<IQueryable<ReservationDto>>;

internal sealed class ReservationGetAllQueryHandler(
    IReservationRepository reservationRepository,
    IQueryable<Customer> customers,
    IQueryable<Branch> branches,
    IQueryable<Vehicle> vehicles,
    IQueryable<Category> categories,
    IQueryable<ProtectionPackage> protectionPackages,
    IQueryable<Extra> extras
    ) : IRequestHandler<ReservationGetAllQuery, IQueryable<ReservationDto>>
{
    public Task<IQueryable<ReservationDto>> Handle(ReservationGetAllQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(
            reservationRepository.GetAllWithAudit()
            .MapTo(
                customers,
                branches,
                vehicles,
                categories,
                protectionPackages,
                extras)
            .AsQueryable());
}
