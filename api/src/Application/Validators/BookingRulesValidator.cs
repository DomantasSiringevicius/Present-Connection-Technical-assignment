using Application.Exceptions;
using Application.Options;
using Application.UseCases.Reservations.CreateReservation;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.Extensions.Options;

namespace Application.Validators;

public class BookingRulesValidator(
    IOptions<BookingOptions> bookingOptions,
    IReservationRepository reservationRepository)
{
    private readonly BookingOptions _bookingOptions = bookingOptions.Value;

    public async Task ValidateAsync(CreateReservationCommand command, Room room)
    {
        var now = DateTime.UtcNow;

        if (command.StartDate <= now)
        {
            throw new BookingRuleException("R4", "Reservation start date must be in the future.");
        }

        if (command.EndDate <= command.StartDate)
        {
            throw new BookingRuleException("R4", "Reservation end date must be after start date.");
        }

        if (command.EndDate - command.StartDate > TimeSpan.FromDays(_bookingOptions.MaxDurationDays))
        {
            throw new BookingRuleException("R1",
                $"Reservation duration cannot exceed {_bookingOptions.MaxDurationDays} days.");
        }

        if (command.StartDate > now.AddDays(_bookingOptions.MaxBookingHorizonDays))
        {
            throw new BookingRuleException("R6",
                $"Reservation cannot be made more than {_bookingOptions.MaxBookingHorizonDays} days in advance.");
        }

        if (!room.IsActive)
        {
            throw new BookingRuleException("R7", $"Cannot book a reservation in an inactive room '{room.Name}'.");
        }

        if (command.NumberOfAttendees > room.Capacity)
        {
            throw new BookingRuleException("R5",
                $"Room '{room.Name}' fits '{room.Capacity}' attendees, but the reservation has '{command.NumberOfAttendees}' attendees.");
        }
        
        if(await reservationRepository.HasOverlappingRoomReservationAsync(room.Id, command.StartDate, command.EndDate))
        {
            throw new BookingRuleException("R2", $"Room '{room.Name}' is already booked for the selected time range.");
        }
        
        if(await reservationRepository.HasOverlappingOrganizerReservationAsync(command.OrganizerId, command.StartDate, command.EndDate))
        {
            throw new BookingRuleException("R3", $"Organizer '{command.OrganizerId}' is already booked for the selected time range.");
        }       
        
    }
}