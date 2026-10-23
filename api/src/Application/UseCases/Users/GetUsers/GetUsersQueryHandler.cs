using Application.Dtos.UserDtos;
using Application.Mappers;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.Users.GetUsers;

public class GetUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUsersQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetUsersAsync();
        
        return users.Select(u => u.ToDto()).ToList();
    }
}