using RentCarServer.Application.Dashboards;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.WebAPI.Modules;

public static class DashboardModule
{
    public static void MapDashboard(this IEndpointRouteBuilder builder)
    {
        var app = builder
            .MapGroup("/dashboard")
            .RequireRateLimiting("fixed")
            .RequireAuthorization()
            .WithTags("Dashboard");

        app.MapGet("active-reservation-count",
            async (ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(new DashboardActiveReservationCountQuery(), cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<int>>();
    }
}
