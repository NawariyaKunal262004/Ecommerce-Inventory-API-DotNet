namespace Medical.Application.Commands;

public class RefreshTokenCommand : IRequest<AuthResponse>
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
