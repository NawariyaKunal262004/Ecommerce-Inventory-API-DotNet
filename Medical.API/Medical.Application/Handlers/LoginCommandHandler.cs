namespace Medical.Application.Handlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;

    public LoginCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _userRepository.LoginAsync(request.UserName, request.Password);
        if (!result.Success || result.Data is not LoginResult login)
            throw new InvalidCredentialsException(result.Message);

        return new AuthResponse
        {
            Token = login.Token,
            RefreshToken = login.RefreshToken,
            RefreshTokenExpiration = login.RefreshTokenExpiration,
            Expiration = login.Expiration,
            UserId = login.UserId,
            UserName = login.UserName,
            Email = login.Email,
            Roles = login.Roles,
            OrganizationId = login.OrganizationId
        };
    }
}
