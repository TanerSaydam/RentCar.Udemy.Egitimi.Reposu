using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using RentCarServer.Application.Branches;
using RentCarServer.Application.Categories;
using RentCarServer.Application.Customers;
using RentCarServer.Application.Extras;
using RentCarServer.Application.ProtectionPackages;
using RentCarServer.Application.Reservations;
using RentCarServer.Application.Roles;
using RentCarServer.Application.Users;
using RentCarServer.Application.Vehicles;
using TS.MediatR;
using CustomerDto = RentCarServer.Application.Customers.CustomerDto;
using VehicleDto = RentCarServer.Application.Vehicles.VehicleDto;

namespace RentCarServer.WebAPI.Controllers;
[Route("odata")]
[ApiController]
[EnableQuery]
public class MainODataController : ODataController
{
    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EnableLowerCamelCase();
        builder.EntitySet<BranchDto>("branches");
        builder.EntitySet<RoleDto>("roles");
        builder.EntitySet<UserDto>("users");
        builder.EntitySet<CategoryDto>("categories");
        builder.EntitySet<ProtectionPackageDto>("protection-packages");
        builder.EntitySet<ExtraDto>("extras");
        builder.EntitySet<VehicleDto>("vehicles");
        builder.EntitySet<CustomerDto>("customers");
        builder.EntitySet<ReservationDto>("reservations");
        return builder.GetEdmModel();
    }

    [HttpGet("branches")]
    public IQueryable<BranchDto> Branches(ISender sender, CancellationToken cancellationToken = default)
        => sender.Send(new BranchGetAllQuery(), cancellationToken).Result;

    [HttpGet("roles")]
    public IQueryable<RoleDto> Roles(ISender sender, CancellationToken cancellationToken = default)
        => sender.Send(new RoleGetAllQuery(), cancellationToken).Result;

    [HttpGet("users")]
    public IQueryable<UserDto> Users(ISender sender, CancellationToken cancellationToken = default)
        => sender.Send(new UserGetAllQuery(), cancellationToken).Result;

    [HttpGet("categories")]
    public IQueryable<CategoryDto> Categories(ISender sender, CancellationToken cancellationToken = default)
        => sender.Send(new CategoryGetAllQuery(), cancellationToken).Result;

    [HttpGet("protection-packages")]
    public IQueryable<ProtectionPackageDto> ProtectionPackages(ISender sender, CancellationToken cancellationToken = default)
        => sender.Send(new ProtectionPackageGetAllQuery(), cancellationToken).Result;

    [HttpGet("extras")]
    public IQueryable<ExtraDto> Extras(ISender sender, CancellationToken cancellationToken = default)
        => sender.Send(new ExtraGetAllQuery(), cancellationToken).Result;

    [HttpGet("vehicles")]
    public IQueryable<VehicleDto> Vehicles(ISender sender, CancellationToken cancellationToken = default)
        => sender.Send(new VehicleGetAllQuery(), cancellationToken).Result;

    [HttpGet("customers")]
    public IQueryable<CustomerDto> Customers(ISender sender, CancellationToken cancellationToken = default)
        => sender.Send(new CustomerGetAllQuery(), cancellationToken).Result;

    [HttpGet("reservations")]
    public async Task<IQueryable<ReservationDto>> Reservations(ISender sender, CancellationToken cancellationToken = default)
    {
        var res = await sender.Send(new ReservationGetAllQuery(), cancellationToken);
        return res;
    }
}
