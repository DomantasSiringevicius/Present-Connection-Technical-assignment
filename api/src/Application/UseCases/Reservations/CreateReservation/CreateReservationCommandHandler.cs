using Application.Dtos.ReservationDtos;
using Application.Mappers;
using Application.Validators;
using Domain.Entities;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.Reservations.CreateReservation;

public class CreateReservationCommandHandler(IReservationRepository reservationRepository,
    IRoomRepository roomRepository,
    IUserRepository userRepository,
    BookingRulesValidator bookingRulesValidator) 
    : IRequestHandler<CreateReservationCommand, ReservationDto>
{
    public async Task<ReservationDto> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetRoomByIdAsync(command.RoomId)
            ?? throw new KeyNotFoundException($"Room with ID {command.RoomId} not found.");
        
        var organizer = await userRepository.GetUserByIdAsync(command.OrganizerId)
            ?? throw new KeyNotFoundException($"User with ID {command.OrganizerId} not found.");

        await bookingRulesValidator.ValidateAsync(command, room);

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            Room = room,
            OrganizerId = organizer.Id,
            Organizer = organizer,
            MeetingTitle = command.MeetingTitle,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            NumberOfAttendees = command.NumberOfAttendees,
        };

        await reservationRepository.AddAsync(reservation);

        return reservation.ToDto();
    }
}