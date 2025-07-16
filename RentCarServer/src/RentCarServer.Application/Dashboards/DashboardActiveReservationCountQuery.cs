using Microsoft.EntityFrameworkCore;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Reservations;
using RentCarServer.Domain.Reservations.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Dashboards;
public sealed record DashboardActiveReservationCountQuery : IRequest<Result<int>>;

internal sealed class DashboardActiveReservationCountQueryHandler(
    IReservationRepository reservationRepository,
    IClaimContext claimContext
    ) : IRequestHandler<DashboardActiveReservationCountQuery, Result<int>>
{
    public async Task<Result<int>> Handle(DashboardActiveReservationCountQuery request, CancellationToken cancellationToken)
    {
        var branchId = claimContext.GetBranchId();
        var res = await reservationRepository
            .GetAll()
            .Where(p => p.PickUpLocationId == branchId)
            .Where(p => p.Status.Value != Status.Completed.Value && p.Status.Value != Status.Canceled.Value)
            .CountAsync(cancellationToken);

        return res;
    }
}