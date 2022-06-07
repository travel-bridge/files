namespace Files.Domain.Exceptions;

public class InvalidRequestException : ExceptionBase
{
    public InvalidRequestException(string message)
        : base("InvalidRequest", 500, false, message)
    {
    }

    public InvalidRequestException(InvalidRequestMessage message) : this(message.Message)
    {
    }
}