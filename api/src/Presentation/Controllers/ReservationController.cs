using Application.Dtos.ReservationDtos;
using Application.UseCases.Reservations.CancelReservation;
using Application.UseCases.Reservations.CreateReservation;
using Application.UseCases.Reservations.EndReservationEarly;
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

    [HttpPost]
    public async Task<ActionResult<ReservationDto>> CreateReservation([FromBody] CreateReservationCommand command)
    {
        var reservation = await mediator.Send(command);
        return CreatedAtAction(nameof(GetReservations), new { id = reservation.Id }, reservation);
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<ActionResult<ReservationDto>> CancelReservation(Guid id)
    {
        return Ok(await mediator.Send(new CancelReservationCommand { Id = id }));
    }

    [HttpPatch("{id:guid}/end-early")]
    public async Task<ActionResult<ReservationDto>> EndReservationEarly(Guid id)
    {
        return Ok(await mediator.Send(new EndReservationEarlyCommand { Id = id }));
    }
}