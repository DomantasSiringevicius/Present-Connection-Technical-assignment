using Application.Dtos.UserDtos;
using Domain.Entities;

namespace Application.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            ReservationsCount = user.Reservations.Count(r => r.IsActive())
        };
    }
}