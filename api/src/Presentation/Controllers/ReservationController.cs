using Application.Dtos.ReservationDtos;
using Application.UseCases.Reservations.GetReservations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/reservations")]
public class ReservationController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ReservationDto>>> GetReservations([FromQuery] GetReservationsQuery query)
    {
        return Ok(await mediator.Send(query));
    }
}