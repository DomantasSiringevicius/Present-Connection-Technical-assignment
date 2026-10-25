namespace Application.Options;

public class BookingOptions
{
    public const string SectioName = "Booking";
    
    public int MaxDurationDays { get; set; }
    
    public int MaxBookingHorizonDays { get; set; }
}