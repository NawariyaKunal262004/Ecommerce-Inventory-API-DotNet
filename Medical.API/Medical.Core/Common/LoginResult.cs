namespace Medical.Core.Common;

public class LoginResult
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiration { get; set; }
    public DateTime Expiration { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public IList<string> Roles { get; set; } = [];
    public Guid OrganizationId { get; set; }
}
