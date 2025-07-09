using Microsoft.AspNetCore.Mvc;
using RentCarServer.Application.Reservations.Forms;
using TS.MediatR;

namespace RentCarServer.WebAPI.Controllers;
[Route("reservation-form")]
[ApiController]
public class FormsController(
    ISender sender) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> Update([FromForm] FormUpdateCommand request, CancellationToken cancellationToken)
    {
        var res = await sender.Send(request, cancellationToken);
        return Ok(res);
    }
}
