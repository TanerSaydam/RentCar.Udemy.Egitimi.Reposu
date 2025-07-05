using RentCarServer.Application.Roles;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.WebAPI.Modules;

public static class RoleModule
{
    public static void MapRole(this IEndpointRouteBuilder builder)
    {
        var app = builder
            .MapGroup("/roles")
            .RequireRateLimiting("fixed")
            .RequireAuthorization()
            .WithTags("Roles");

        app.MapPost(string.Empty,
            async (RoleCreateCommand request, ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(request, cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<string>>();

        app.MapPut(string.Empty,
            async (RoleUpdateCommand request, ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(request, cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<string>>();

        app.MapDelete("{id}",
            async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(new RoleDeleteCommand(id), cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<string>>();

        app.MapGet("{id}",
            async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(new RoleGetQuery(id), cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<RoleDto>>();

        app.MapPut("update-permissions",
            async (RoleUpdatePermissionsCommand request, ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(request, cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<string>>();
    }
}
