using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Medical.Application.Handlers;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IConfiguration configuration)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // Get the refresh token from database
        var refreshToken = await _refreshTokenRepository.GetActiveByTokenAsync(request.RefreshToken);
        if (refreshToken == null)
            throw new InvalidCredentialsException("Invalid refresh token.");

        // Check if refresh token is expired
        if (refreshToken.IsExpired)
            throw new InvalidCredentialsException("Refresh token has expired.");

        // Get the user associated with the refresh token
        var user = refreshToken.User;
        if (user == null)
            throw new UserNotFoundException();

        // Validate the JWT token matches the refresh token
        var jwtHandler = new JwtSecurityTokenHandler();
        if (!jwtHandler.CanReadToken(request.Token))
            throw new InvalidCredentialsException("Invalid access token.");

        var jwtToken = jwtHandler.ReadJwtToken(request.Token);
        if (jwtToken.Id != refreshToken.JwtId)
            throw new InvalidCredentialsException("Token mismatch.");

        // Get user roles
        var userManager = _userRepository.GetUserManager();
        if (userManager == null)
            throw new InvalidOperationException("UserManager not available.");
        
        var roles = await userManager.GetRolesAsync(user);

        // Generate new JWT token
        var expirationMinutes = int.TryParse(
            _configuration[CommonFields.JwtColonExpirationMinutes], out var minutes)
            ? minutes
            : 60;
        var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var newJwtToken = GenerateJwtToken(user, roles, expiration);

        // Generate new refresh token (rotation)
        var newRefreshToken = GenerateRefreshToken(newJwtToken);
        newRefreshToken.UserId = user.Id;

        // Revoke old refresh token
        await _refreshTokenRepository.RevokeAsync(refreshToken, replacedByToken: newRefreshToken.Token);

        // Save new refresh token
        await _refreshTokenRepository.CreateAsync(newRefreshToken);

        return new AuthResponse
        {
            Token = newJwtToken,
            RefreshToken = newRefreshToken.Token,
            RefreshTokenExpiration = newRefreshToken.ExpiresAt,
            Expiration = expiration,
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Roles = roles.ToList(),
            OrganizationId = user.OrganizationId
        };
    }

    private string GenerateJwtToken(ApplicationUser user, IList<string> roles, DateTime expiration)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")
                ?? throw new InvalidOperationException("JWT_KEY environment variable is required.")));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(CommonFields.UserId, user.Id),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(CommonFields.Username, user.UserName ?? string.Empty),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(CommonFields.OrganizationId, user.OrganizationId.ToString())
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        if (roles.Count == 0 && !string.IsNullOrWhiteSpace(user.Role))
            claims.Add(new Claim(ClaimTypes.Role, user.Role));

        var token = new JwtSecurityToken(
            _configuration[CommonFields.JwtColonIssuer],
            _configuration[CommonFields.JwtColonAudience],
            claims,
            expires: expiration,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private RefreshTokenEntity GenerateRefreshToken(string jwtToken)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var refreshToken = Convert.ToBase64String(randomNumber);

        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtTokenObj = jwtHandler.ReadJwtToken(jwtToken);
        var jwtId = string.IsNullOrWhiteSpace(jwtTokenObj.Id) ? Guid.NewGuid().ToString() : jwtTokenObj.Id;

        var refreshTokenExpirationDays = int.TryParse(
            _configuration["Jwt:RefreshTokenExpirationDays"], out var days)
            ? days
            : 7;

        return new RefreshTokenEntity
        {
            Token = refreshToken,
            JwtId = jwtId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationDays)
        };
    }
}
