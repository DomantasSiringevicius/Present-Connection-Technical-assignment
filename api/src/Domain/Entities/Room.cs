using Domain.Enums;

namespace Domain.Entities;

public class Room
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required int Capacity { get; set; }

    public required int Floor { get; set; }

    public HashSet<Equipment> Equipments { get; set; } = [];

    public required bool IsActive { get; set; }

    public ICollection<Reservation> Reservations { get; set; } = [];
}