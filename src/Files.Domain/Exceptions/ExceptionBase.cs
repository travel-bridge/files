namespace Files.Domain.Exceptions;

public abstract class ExceptionBase : Exception
{
    protected ExceptionBase(
        string category,
        int statusCode,
        bool isLocalized,
        string message,
        params string[] messageParameters) : base(message)
    {
        Category = category;
        StatusCode = statusCode;
        IsLocalized = isLocalized;
        MessageParameters = messageParameters.ToList().AsReadOnly();
    }

    public string Category { get; }

    public int StatusCode { get; }

    public bool IsLocalized { get; }

    public IReadOnlyCollection<string> MessageParameters { get; }
}