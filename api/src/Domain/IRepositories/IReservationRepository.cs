using Domain.Entities;
using Domain.Enums;

namespace Domain.IRepositories;

public interface IReservationRepository
{
    Task<List<Reservation>> GetReservationsAsync(Guid? roomId, Guid? organizerId, ReservationStatus? status, DateTime? startDate, DateTime? endDate);
    
    Task AddAsync(Reservation reservation);
    
    Task<bool> HasOverlappingRoomReservationAsync(Guid roomId, DateTime startDate, DateTime endDate);
    
    Task<bool> HasOverlappingOrganizerReservationAsync(Guid organizerId, DateTime startDate, DateTime endDate);
}