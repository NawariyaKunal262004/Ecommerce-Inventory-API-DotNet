namespace Medical.Application.Responses;

public class UserResponse
{
    public string Id { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Role { get; set; }
    public Guid OrganizationId { get; set; }
}
