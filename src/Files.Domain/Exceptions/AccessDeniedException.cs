namespace Files.Domain.Exceptions;

public class AccessDeniedException : ExceptionBase
{
    public AccessDeniedException(string message)
        : base("AccessDenied", 403, false, message)
    {
    }
}