using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using RentCarServer.Application.Branches;
using TS.MediatR;

namespace RentCarServer.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableQuery]
public class ODataController : ControllerBase
{
    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EnableLowerCamelCase();
        builder.EntitySet<BranchGetAllQueryResponse>("branches");
        return builder.GetEdmModel();
    }

    public Task<IQueryable<BranchGetAllQueryResponse>> Branches(ISender sender, CancellationToken cancellationToken = default)
        => sender.Send(new BranchGetAllQuery(), cancellationToken);
}
