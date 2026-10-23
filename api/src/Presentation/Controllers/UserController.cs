using Application.Dtos.UserDtos;
using Application.UseCases.Users.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetUsers([FromQuery] GetUsersQuery query)
    {
        return Ok(await mediator.Send(query));
    }
}