using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seeders.EntitySeeders;

public class RoomSeeder : ISeeder
{
    public async Task SeedAsync(AppDbContext dbContext, IServiceProvider services)
    {
        if (await dbContext.Rooms.AnyAsync())
        {
            return;
        }

        var rooms = await GetRooms(dbContext);

        await dbContext.Rooms.AddRangeAsync(rooms);
        await dbContext.SaveChangesAsync();
    }

    private static async Task<List<Room>> GetRooms(AppDbContext dbContext)
    {
        return
        [
            new Room()
            {
                Id = Guid.NewGuid(),
                Name = "Room 1",
                Capacity = 2,
                Floor = 1,
                Equipments = [Equipment.TvScreen, Equipment.Projector],
                IsActive = true
            },
            new Room()
            {
                Id = Guid.NewGuid(),
                Name = "Room 2",
                Capacity = 4,
                Floor = 2,
                Equipments = [Equipment.TvScreen, Equipment.Whiteboard, Equipment.Projector],
                IsActive = false
            },
            new Room()
            {
                Id = Guid.NewGuid(),
                Name = "Room 3",
                Capacity = 6,
                Floor = 2,
                Equipments = [],
                IsActive = true
            },
            new Room()
            {
                Id = Guid.NewGuid(),
                Name = "Room 4",
                Capacity = 8,
                Floor = 2,
                Equipments = [Equipment.TvScreen, Equipment.Whiteboard],
                IsActive = true
            },
            new Room()
            {
                Id = Guid.NewGuid(),
                Name = "Room 5",
                Capacity = 10,
                Floor = 3,
                Equipments = [Equipment.TvScreen, Equipment.Whiteboard, Equipment.Projector],
                IsActive = false
            }
        ];
    }
}