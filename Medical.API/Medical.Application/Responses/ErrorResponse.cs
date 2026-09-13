namespace Medical.Application.Responses;
public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? ErrorCode { get; set; }
    public string? TraceId { get; set; }
    public IEnumerable<ValidationErrorModel>? ValidationErrors { get; set; }
}
