namespace Files.Domain.Exceptions;

public class DomainException : ExceptionBase
{
    public DomainException(string message, params string[] messageParameters)
        : base("Domain", true, message, messageParameters)
    {
    }
}