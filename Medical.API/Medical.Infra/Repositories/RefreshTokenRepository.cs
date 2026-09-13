namespace Medical.Infra.Repositories;

public class RefreshTokenRepository : RepositoryBase<RefreshTokenEntity>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext context, ILogger<RepositoryBase<RefreshTokenEntity>> logger)
        : base(context, logger)
    {
    }

    public async Task<RefreshTokenEntity?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == token);
    }

    public async Task<RefreshTokenEntity?> GetActiveByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == token && r.IsActive);
    }

    public async Task<IEnumerable<RefreshTokenEntity>> GetByUserIdAsync(string userId)
    {
        return await _context.RefreshTokens
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<RefreshTokenEntity> CreateAsync(RefreshTokenEntity refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
        return refreshToken;
    }

    public async Task RevokeAsync(RefreshTokenEntity refreshToken, string? revokedByIp = null, string? replacedByToken = null)
    {
        refreshToken.RevokedAt = DateTime.UtcNow;
        refreshToken.RevokedByIp = revokedByIp;
        refreshToken.ReplacedByToken = replacedByToken;
        _context.RefreshTokens.Update(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task RevokeAllUserTokensAsync(string userId, string? revokedByIp = null)
    {
        var tokens = await _context.RefreshTokens
            .Where(r => r.UserId == userId && r.IsActive)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.RevokedAt = DateTime.UtcNow;
            token.RevokedByIp = revokedByIp;
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteExpiredTokensAsync()
    {
        var expiredTokens = await _context.RefreshTokens
            .Where(r => r.IsExpired)
            .ToListAsync();

        _context.RefreshTokens.RemoveRange(expiredTokens);
        await _context.SaveChangesAsync();
    }
}
