namespace Domain.Entities;

public class User
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required string LastName { get; set; }

    public ICollection<Reservation> Reservations { get; set; } = [];
}