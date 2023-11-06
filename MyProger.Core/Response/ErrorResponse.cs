namespace MyProger.Core.Response;

public class ErrorResponse
{
    public required string Message { get; set; }
    public required string ErrorCode { get; set; }
}