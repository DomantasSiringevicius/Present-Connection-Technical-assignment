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
            throw new BookingRuleException("R5", "Reservation end date must be after start date.");
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
        
    }
}