using Domain.Entities;
using Domain.Enums;

namespace Domain.IRepositories;

public interface IRoomRepository
{
    Task<Room?> GetRoomByIdAsync(Guid id);

    Task<List<Room>?> GetRoomsAsync(int? minCapacity, int? floor, List<Equipment>? equipment);
}