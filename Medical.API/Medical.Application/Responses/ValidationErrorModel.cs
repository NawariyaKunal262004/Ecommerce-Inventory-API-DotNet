namespace Medical.Application.Responses;
public class ValidationErrorModel
{
    public string PropertyName { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}
