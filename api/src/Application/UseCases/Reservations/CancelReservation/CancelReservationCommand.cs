using Application.Dtos.ReservationDtos;
using MediatR;

namespace Application.UseCases.Reservations.CancelReservation;

public class CancelReservationCommand : IRequest<ReservationDto>
{
    public required Guid Id { get; set; }
}