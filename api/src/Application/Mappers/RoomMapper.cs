using Application.Dtos;
using Domain.Entities;

namespace Application.Mappers;

public static class RoomMapper
{
    public static RoomDto ToDto(this Room room)
    {
        return new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            Floor = room.Floor,
            Equipment = room.Equipments,
            IsActive = room.IsActive
        };
    }
}