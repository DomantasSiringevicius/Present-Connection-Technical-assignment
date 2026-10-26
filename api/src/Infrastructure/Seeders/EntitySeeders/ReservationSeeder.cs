using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seeders.EntitySeeders;

public class ReservationSeeder : ISeeder
{
    public async Task SeedAsync(AppDbContext dbContext, IServiceProvider services)
    {
        if (await dbContext.Reservations.AnyAsync())
        {
            return;
        }

        var rooms = await dbContext.Rooms.ToDictionaryAsync(r => r.Name);
        var users = await dbContext.Users.ToDictionaryAsync(u => u.Name);

        dbContext.Reservations.AddRange(GetReservations(rooms, users));
        await dbContext.SaveChangesAsync();
    }

    private static List<Reservation> GetReservations(
        Dictionary<string, Room> rooms,
        Dictionary<string, User> users)
    {
        var today = DateTime.UtcNow.Date;
        var now = DateTime.UtcNow;

        return
        [
            new Reservation
            {
                Id = Guid.NewGuid(),
                RoomId = rooms["Room 4"].Id,
                OrganizerId = users["Jonas"].Id,
                MeetingTitle = "Sprint planning",
                StartDate = today.AddDays(1).AddHours(9),
                EndDate = today.AddDays(1).AddHours(11),
                NumberOfAttendees = 8
            },
            new Reservation
            {
                Id = Guid.NewGuid(),
                RoomId = rooms["Room 3"].Id,
                OrganizerId = users["Jonas"].Id,
                MeetingTitle = "Daily stand-up",
                StartDate = today.AddDays(1).AddHours(11).AddMinutes(30),
                EndDate = today.AddDays(1).AddHours(11).AddMinutes(45),
                NumberOfAttendees = 4
            },
            new Reservation
            {
                Id = Guid.NewGuid(),
                RoomId = rooms["Room 3"].Id,
                OrganizerId = users["Jonas"].Id,
                MeetingTitle = "Design review",
                StartDate = now.AddMinutes(-30),
                EndDate = today.AddHours(1),
                NumberOfAttendees = 5
            },
            new Reservation
            {
                Id = Guid.NewGuid(),
                RoomId = rooms["Room 4"].Id,
                OrganizerId = users["Ona"].Id,
                MeetingTitle = "Client demo",
                StartDate = today.AddDays(2).AddHours(13),
                EndDate = today.AddDays(2).AddHours(15),
                NumberOfAttendees = 7
            },
            new Reservation
            {
                Id = Guid.NewGuid(),
                RoomId = rooms["Room 1"].Id,
                OrganizerId = users["Petras"].Id,
                MeetingTitle = "1:1",
                StartDate = today.AddDays(3).AddHours(10),
                EndDate = today.AddDays(3).AddHours(10).AddMinutes(30),
                NumberOfAttendees = 2
            },

            new Reservation
            {
                Id = Guid.NewGuid(),
                RoomId = rooms["Room 3"].Id,
                OrganizerId = users["Rūta"].Id,
                MeetingTitle = "Retrospective",
                StartDate = today.AddDays(-2).AddHours(14),
                EndDate = today.AddDays(-2).AddHours(16),
                NumberOfAttendees = 5,
                ReservationStatus = ReservationStatus.Completed
            },
            new Reservation
            {
                Id = Guid.NewGuid(),
                RoomId = rooms["Room 4"].Id,
                OrganizerId = users["Ona"].Id,
                MeetingTitle = "Backlog refinement",
                StartDate = today.AddDays(-3).AddHours(10),
                EndDate = today.AddDays(-3).AddHours(12),
                ActualEndDate = today.AddDays(-3).AddHours(10).AddMinutes(40),
                NumberOfAttendees = 8,
                ReservationStatus = ReservationStatus.Completed
            },
            new Reservation
            {
                Id = Guid.NewGuid(),
                RoomId = rooms["Room 4"].Id,
                OrganizerId = users["Petras"].Id,
                MeetingTitle = "Team workshop",
                StartDate = today.AddDays(-5).AddHours(9),
                EndDate = today.AddDays(-5).AddHours(17),
                ActualEndDate = today.AddDays(-5).AddHours(12),
                NumberOfAttendees = 8,
                ReservationStatus = ReservationStatus.Completed
            },

            new Reservation
            {
                Id = Guid.NewGuid(),
                RoomId = rooms["Room 4"].Id,
                OrganizerId = users["Rūta"].Id,
                MeetingTitle = "Architecture review",
                StartDate = today.AddDays(1).AddHours(9),
                EndDate = today.AddDays(1).AddHours(11),
                NumberOfAttendees = 6,
                ReservationStatus = ReservationStatus.Cancelled
            }
        ];
    }
}