namespace Application.Dtos.UserDtos;

public class UserDto
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required string LastName { get; set; }

    public int ReservationsCount { get; set; }
}