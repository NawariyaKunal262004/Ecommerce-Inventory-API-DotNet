namespace Medical.Application.Commands;

public class LoginCommand : IRequest<AuthResponse>
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
