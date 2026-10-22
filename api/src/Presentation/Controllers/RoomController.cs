using Application.Dtos;
using Application.UseCases.Rooms.GetRooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<RoomDto>>> GetRooms([FromQuery] GetRoomsQuery query)
    {
        return Ok(await mediator.Send(query));
    }
}