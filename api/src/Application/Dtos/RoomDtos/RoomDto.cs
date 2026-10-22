using Domain.Enums;

namespace Application.Dtos;

public class RoomDto
{
    public required Guid Id { get; set; }
    
    public required string Name { get; set; }

    public required int Capacity { get; set; }

    public required int Floor { get; set; }

    public required List<Equipment> Equipment { get; set; }
    
    public required bool IsActive { get; set; }
}