using Application.Dtos.ReservationDtos;
using Domain.Enums;
using MediatR;

namespace Application.UseCases.Reservations.GetReservations;

public class GetReservationsQuery : IRequest<List<ReservationDto>>
{
    public Guid? OrganizerId { get; set; }
    
    public Guid? RoomId { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public ReservationStatus? Status { get; set; }
}