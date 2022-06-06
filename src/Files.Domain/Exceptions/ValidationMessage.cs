namespace Files.Domain.Exceptions;

public class ValidationMessage
{
    public ValidationMessage(
        IEnumerable<string> location,
        string message,
        params string[] messageParameters)
    {
        Location = location.ToList().AsReadOnly();
        Message = message;
        MessageParameters = messageParameters.ToList().AsReadOnly();
    }

    public IReadOnlyCollection<string> Location { get; }

    public string Message { get; }

    public IReadOnlyCollection<string> MessageParameters { get; }
}