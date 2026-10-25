using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReservationRepository(AppDbContext dbContext) : IReservationRepository
{
    public async Task<List<Reservation>> GetReservationsAsync(Guid? roomId, Guid? organizerId,
        ReservationStatus? status, DateTime? startDate, DateTime? endDate)
    {
        var query = dbContext.Reservations
            .Include(r => r.Room)
            .Include(r => r.Organizer)
            .AsQueryable();


        if (roomId.HasValue)
        {
            query = query.Where(r => r.RoomId == roomId.Value);
        }

        if (organizerId.HasValue)
        {
            query = query.Where(r => r.OrganizerId == organizerId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(r => r.ReservationStatus == status.Value);
        }

        if (startDate.HasValue)
        {
            query = query.Where(r => r.EndDate > startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(r => r.StartDate < endDate.Value);
        }

        return await query.OrderBy(r => r.StartDate)
            .ToListAsync();
    }

    public async Task AddAsync(Reservation reservation)
    {
        await dbContext.Reservations.AddAsync(reservation);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> HasOverlappingRoomReservationAsync(Guid roomId, DateTime startDate, DateTime endDate)
    {
        return await dbContext.Reservations.AnyAsync(r => r.RoomId == roomId &&
                                                          r.ReservationStatus != ReservationStatus.Cancelled &&
                                                          r.StartDate < endDate &&
                                                          (r.ActualEndDate ?? r.EndDate) > startDate);
    }

    public async Task<bool> HasOverlappingOrganizerReservationAsync(Guid organizerId, DateTime startDate,
        DateTime endDate)
    {
        return await dbContext.Reservations.AnyAsync(r => r.OrganizerId == organizerId &&
                                                          r.ReservationStatus != ReservationStatus.Cancelled &&
                                                          r.StartDate < endDate &&
                                                          (r.ActualEndDate ?? r.EndDate) > startDate);
    }
}