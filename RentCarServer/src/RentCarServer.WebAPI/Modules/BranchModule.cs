using RentCarServer.Application.Branches;
using RentCarServer.Domain.Branches;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.WebAPI.Modules;

public static class BranchModule
{
    public static void MapBranch(this IEndpointRouteBuilder builder)
    {
        var app = builder
            .MapGroup("/branches")
            .RequireRateLimiting("fixed")
            .RequireAuthorization();

        app.MapPost(string.Empty,
            async (BranchCreateCommand request, ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(request, cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<string>>();

        app.MapPut(string.Empty,
            async (BranchUpdateCommand request, ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(request, cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<string>>();

        app.MapDelete("{id}",
            async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(new BranchDeleteCommand(id), cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<string>>();

        app.MapGet("{id}",
            async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(new BranchGetQuery(id), cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<Branch>>();
    }
}
