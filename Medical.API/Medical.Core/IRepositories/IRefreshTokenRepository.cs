namespace Medical.Core.IRepositories;

public interface IRefreshTokenRepository
{
    Task<RefreshTokenEntity?> GetByTokenAsync(string token);
    Task<RefreshTokenEntity?> GetActiveByTokenAsync(string token);
    Task<IEnumerable<RefreshTokenEntity>> GetByUserIdAsync(string userId);
    Task<RefreshTokenEntity> CreateAsync(RefreshTokenEntity refreshToken);
    Task RevokeAsync(RefreshTokenEntity refreshToken, string? revokedByIp = null, string? replacedByToken = null);
    Task RevokeAllUserTokensAsync(string userId, string? revokedByIp = null);
    Task DeleteExpiredTokensAsync();
}
