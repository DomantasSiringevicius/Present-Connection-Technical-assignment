namespace Application.Exceptions;

public class BookingRuleException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}