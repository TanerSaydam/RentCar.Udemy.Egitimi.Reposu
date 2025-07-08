using RentCarServer.Application.Reservations;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.WebAPI.Modules;

public static class ReservationModule
{
    public static void MapReservation(this IEndpointRouteBuilder builder)
    {
        var app = builder
            .MapGroup("/reservations")
            .RequireRateLimiting("fixed")
            .RequireAuthorization()
            .WithTags("Reservations");

        app.MapPost(string.Empty,
            async (ReservationCreateCommand request, ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(request, cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<string>>();

        app.MapPut(string.Empty,
            async (ReservationUpdateCommand request, ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(request, cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<string>>();

        app.MapDelete("{id}",
            async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(new ReservationDeleteCommand(id), cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<string>>();

        app.MapGet("{id}",
            async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(new ReservationGetQuery(id), cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<ReservationDto>>();
    }
}
