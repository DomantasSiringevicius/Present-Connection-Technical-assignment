using Application.Dtos.ReservationDtos;
using Application.Mappers;
using Domain.Enums;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.Reservations.CancelReservation;

public class CancelReservationCommandHandler(IReservationRepository reservationRepository)
    : IRequestHandler<CancelReservationCommand, ReservationDto>
{
    public async Task<ReservationDto> Handle(CancelReservationCommand command, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetReservationByIdAsync(command.Id)
                          ?? throw new KeyNotFoundException($"Reservation with ID {command.Id} not found.");

        if (reservation.ReservationStatus != ReservationStatus.Booked || reservation.EndDate <= DateTime.UtcNow)
        {
            throw new InvalidOperationException("Only ongoing or upcoming booked reservations can be cancelled.");
        }

        reservation.ReservationStatus = ReservationStatus.Cancelled;
        await reservationRepository.UpdateAsync(reservation);

        return reservation.ToDto();
    }
}