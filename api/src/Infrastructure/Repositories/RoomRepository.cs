using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoomRepository(AppDbContext dbContext) : IRoomRepository
{
    public async Task<Room?> GetRoomByIdAsync(Guid id)
    {
        return await dbContext.Rooms.FirstOrDefaultAsync(rm => rm.Id.Equals(id));
    }

    public async Task<List<Room>> GetRoomsAsync(int? minCapacity, int? floor, List<Equipment>? equipment)
    {
        var query = dbContext.Rooms.AsQueryable();

        if (minCapacity.HasValue)
        {
            query = query.Where(rm => rm.Capacity >= minCapacity.Value);
        }

        if (floor.HasValue)
        {
            query = query.Where(rm => rm.Floor == floor.Value);
        }

        if (equipment != null && equipment.Any())
        {
            foreach (var eq in equipment)
            {
                query = query.Where(rm => rm.Equipments.Contains(eq));
            }
        }

        return await query.ToListAsync();
    }
}