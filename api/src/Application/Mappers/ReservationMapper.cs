using Application.Dtos.ReservationDtos;
using Domain.Entities;

namespace Application.Mappers;

public static class ReservationMapper
{
    public static ReservationDto ToDto(this Reservation reservation)
    {
        return new ReservationDto
        {
            Id = reservation.Id,
            MeetingTitle = reservation.MeetingTitle,
            RoomId = reservation.RoomId,
            RoomName = reservation.Room.Name,
            OrganizerId = reservation.OrganizerId,
            OrganizerName = reservation.Organizer.Name,
            OrganizerLastName = reservation.Organizer.LastName,
            StartDate = reservation.StartDate,
            EndDate = reservation.EndDate,
            ActualEndDate = reservation.ActualEndDate,
            NumberOfAttendees = reservation.NumberOfAttendees,
            ReservationStatus = reservation.ReservationStatus
        };
    }
}