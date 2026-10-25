using Application.Dtos.ReservationDtos;
using Application.Mappers;
using Domain.Entities;
using Domain.IRepositories;
using MediatR;

namespace Application.UseCases.Reservations.CreateReservation;

public class CreateReservationCommandHandler(IReservationRepository reservationRepository,
    IRoomRepository roomRepository,
    IUserRepository userRepository) 
    : IRequestHandler<CreateReservationCommand, ReservationDto>
{
    public async Task<ReservationDto> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetRoomByIdAsync(request.RoomId)
            ?? throw new KeyNotFoundException($"Room with ID {request.RoomId} not found.");
        
        var organizer = await userRepository.GetUserByIdAsync(request.OrganizerId)
            ?? throw new KeyNotFoundException($"User with ID {request.OrganizerId} not found.");

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            Room = room,
            OrganizerId = organizer.Id,
            Organizer = organizer,
            MeetingTitle = request.MeetingTitle,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            NumberOfAttendees = request.NumberOfAttendees,
        };

        await reservationRepository.AddAsync(reservation);

        return reservation.ToDto();
    }
}