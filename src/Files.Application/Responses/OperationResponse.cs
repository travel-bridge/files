namespace Files.Application.Responses;

public class OperationResponse
{
    public static OperationResponse Success = new OperationResponse { IsSuccess = true };
    
    public static OperationResponse NotSuccess = new OperationResponse { IsSuccess = false };
    
    public bool IsSuccess { get; init; }
}