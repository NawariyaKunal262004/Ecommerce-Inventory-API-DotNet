namespace Medical.Core.Entity;

public class RefreshTokenEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Token { get; set; } = string.Empty;

    public string JwtId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public string? RevokedByIp { get; set; }

    public string? ReplacedByToken { get; set; }

    public string? UserId { get; set; }

    public virtual ApplicationUser? User { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    public bool IsRevoked => RevokedAt != null;

    public bool IsActive => !IsRevoked && !IsExpired;
}
