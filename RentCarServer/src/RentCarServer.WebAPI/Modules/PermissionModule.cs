using RentCarServer.Application.Permissions;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.WebAPI.Modules;

public static class PermissionModule
{
    public static void MapPermission(this IEndpointRouteBuilder builder)
    {
        var app = builder
            .MapGroup("/permissions")
            .RequireRateLimiting("fixed")
            .RequireAuthorization()
            .WithTags("Permissons");

        app.MapGet(string.Empty,
            async (ISender sender, CancellationToken cancellationToken) =>
            {
                var res = await sender.Send(new PermissionGetAllQuery(), cancellationToken);
                return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
            })
            .Produces<Result<List<string>>>();
    }
}