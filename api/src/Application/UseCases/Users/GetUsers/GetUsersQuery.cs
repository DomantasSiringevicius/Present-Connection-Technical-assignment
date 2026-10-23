using Application.Dtos.UserDtos;
using MediatR;

namespace Application.UseCases.Users.GetUsers;

public class GetUsersQuery : IRequest<List<UserDto>>
{
    
}