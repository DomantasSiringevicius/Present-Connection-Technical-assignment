using Application.Dtos.ReservationDtos;
using MediatR;

namespace Application.UseCases.Reservations.EndReservationEarly;

public class EndReservationEarlyCommand : IRequest<ReservationDto>
{
    public required Guid Id { get; set; }
}