namespace Files.Domain.Exceptions;

public abstract class ExceptionBase : Exception
{
    protected ExceptionBase(
        string category,
        bool isLocalized,
        string message,
        params string[] messageParameters) : base(message)
    {
        Category = category;
        IsLocalized = isLocalized;
        MessageParameters = messageParameters.ToList().AsReadOnly();
    }

    public string Category { get; }

    public bool IsLocalized { get; }

    public IReadOnlyCollection<string> MessageParameters { get; }
}