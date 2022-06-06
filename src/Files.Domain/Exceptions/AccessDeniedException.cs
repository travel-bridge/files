namespace Files.Domain.Exceptions;

public class AccessDeniedException : ExceptionBase
{
    public AccessDeniedException(string message)
        : base("AccessDenied", false, message)
    {
    }
}