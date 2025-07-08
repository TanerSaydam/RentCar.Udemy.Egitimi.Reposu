using Microsoft.EntityFrameworkCore;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Categories;
using RentCarServer.Domain.Customers;
using RentCarServer.Domain.Extras;
using RentCarServer.Domain.ProtectionPackages;
using RentCarServer.Domain.Reservations;
using RentCarServer.Domain.Vehicles;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Reservations;
public sealed record ReservationGetQuery(
    Guid Id) : IRequest<Result<ReservationDto>>;

internal sealed class ReservationGetQueryHandler(
    IReservationRepository reservationRepository,
    IQueryable<Customer> customers,
    IQueryable<Branch> branches,
    IQueryable<Vehicle> vehicles,
    IQueryable<Category> categories,
    IQueryable<ProtectionPackage> protectionPackages,
    IQueryable<Extra> extras
    ) : IRequestHandler<ReservationGetQuery, Result<ReservationDto>>
{
    public async Task<Result<ReservationDto>> Handle(ReservationGetQuery request, CancellationToken cancellationToken)
    {
        var res = await reservationRepository.GetAllWithAudit().MapTo(
            customers,
            branches,
            vehicles,
            categories,
            protectionPackages,
            extras)
            .Where(i => i.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (res is null)
        {
            return Result<ReservationDto>.Failure("Rezervasyon bulunamadı");
        }

        return res;
    }
}
