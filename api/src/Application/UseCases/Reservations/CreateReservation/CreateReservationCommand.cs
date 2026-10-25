using Application.Dtos.ReservationDtos;
using MediatR;

namespace Application.UseCases.Reservations.CreateReservation;

public class CreateReservationCommand: IRequest<ReservationDto>
{
    public required Guid RoomId { get; set; }
    
    public required Guid OrganizerId { get; set; }
    
    public required string MeetingTitle { get; set; }
    
    public required DateTime StartDate { get; set; }
    
    public required DateTime EndDate { get; set; }
    
    public required int NumberOfAttendees { get; set; }
}