namespace Files.Domain.Exceptions;

public class InvalidRequestException : ExceptionBase
{
    public InvalidRequestException(string message)
        : base("InvalidRequest", false, message)
    {
    }

    public InvalidRequestException(InvalidRequestMessage message) : this(message.Message)
    {
    }
}