namespace Medical.Application.Handlers;

public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, bool>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RevokeTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
        if (refreshToken == null)
            throw new InvalidCredentialsException("Invalid refresh token.");

        if (refreshToken.IsRevoked)
            return true; // Already revoked

        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        await _refreshTokenRepository.RevokeAsync(refreshToken, revokedByIp: ipAddress);

        return true;
    }
}
