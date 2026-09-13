namespace Medical.Core.Common;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Role { get; set; }
    public Guid OrganizationId { get; set; }
}
