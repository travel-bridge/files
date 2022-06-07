namespace Files.Domain.Exceptions;

public class ValidationException : ExceptionBase
{
    public ValidationException(IEnumerable<ValidationMessage> messages)
        : base("Validation", 400, true, "Validation")
    {
        Messages = messages.ToList().AsReadOnly();
    }

    public ValidationException(ValidationMessage message)
        : this(new[] { message })
    {
    }

    public ValidationException(
        IEnumerable<string> location,
        string message,
        params string[] messageParameters)
        : this(new ValidationMessage(location, message, messageParameters))
    {
    }

    public IReadOnlyCollection<ValidationMessage> Messages { get; }
}