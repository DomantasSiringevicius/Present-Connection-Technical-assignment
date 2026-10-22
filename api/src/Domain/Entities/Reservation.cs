using Domain.Enums;

namespace Domain.Entities;

public class Reservation
{
    public required Guid Id { get; set; }

    public User? Organizer { get; set; } = null!;

    public Room? Room { get; set; } = null!;

    public required Guid RoomId { get; set; }

    public required Guid OrganizerId { get; set; }

    public required string MeetingTitle { get; set; }

    public required DateTime StartDate { get; set; }

    public required DateTime EndDate { get; set; }

    public DateTime? ActualEndDate { get; set; }

    public required int NumberOfAttendees { get; set; }

    public ReservationStatus ReservationStatus { get; set; } = ReservationStatus.Booked;
}