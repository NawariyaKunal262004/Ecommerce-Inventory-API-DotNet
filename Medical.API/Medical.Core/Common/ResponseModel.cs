namespace Medical.Core.Common;
public class ResponseModel
{
    public bool Success { get; set; }

    public string? Message { get; set; }

    public object? Data { get; set; }
    public string RoleName { get; set; }
    public object OrganizationId { get; set; }

    public ResponseModel() { }

    public ResponseModel(bool success, string? message = null, object? data = null)
    {
        Success = success;
        Message = message;
        Data = data;
    }

    public static ResponseModel SuccessResponse(object? data = null, string? message = "Operation successful")
    {
        return new ResponseModel(true, message, data);
    }

    public static ResponseModel FailureResponse(string? message = "Operation failed")
    {
        return new ResponseModel(false, message);
    }
}
