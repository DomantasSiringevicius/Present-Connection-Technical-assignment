using Application.Dtos.ReservationDtos;
using Application.Mappers;
using Domain.Enums;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.Reservations.EndReservationEarly;

public class EndReservationEarlyCommandHandler(IReservationRepository reservationRepository) : IRequestHandler<EndReservationEarlyCommand, ReservationDto>
{
    public async Task<ReservationDto> Handle(EndReservationEarlyCommand command, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetReservationByIdAsync(command.Id)
            ?? throw new KeyNotFoundException($"Reservation with ID {command.Id} not found.");
        
        var now = DateTime.UtcNow;
        
        if(reservation.ReservationStatus != ReservationStatus.Booked || reservation.StartDate > now || reservation.EndDate < now)
        {
            throw new InvalidOperationException("Only active reservations can be ended early.");
        }
        
        reservation.ActualEndDate = now;
        reservation.ReservationStatus = ReservationStatus.Completed;
        await reservationRepository.UpdateAsync(reservation);

        return reservation.ToDto();
    }
}