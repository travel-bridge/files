namespace Files.Application.Responses;

public class OperationResponse
{
    public static OperationResponse Success = new() { IsSuccess = true };
    
    public static OperationResponse NotSuccess = new() { IsSuccess = false };
    
    public bool IsSuccess { get; init; }
}