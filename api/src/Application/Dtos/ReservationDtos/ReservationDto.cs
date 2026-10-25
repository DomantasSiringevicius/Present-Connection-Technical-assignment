using Domain.Enums;

namespace Application.Dtos.ReservationDtos;

public class ReservationDto
{
    public required Guid Id { get; set; }

    public required string MeetingTitle { get; set; }

    public required Guid RoomId { get; set; }

    public required string RoomName { get; set; }

    public required Guid OrganizerId { get; set; }

    public required string OrganizerName { get; set; }

    public required DateTime StartDate { get; set; }

    public required DateTime EndDate { get; set; }

    public DateTime? ActualEndDate { get; set; }

    public required int NumberOfAttendees { get; set; }

    public ReservationStatus ReservationStatus { get; set; }
}