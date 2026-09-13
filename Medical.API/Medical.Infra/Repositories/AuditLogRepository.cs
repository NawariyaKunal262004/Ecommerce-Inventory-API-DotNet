namespace Medical.Infra.Repositories;

public class AuditLogRepository : RepositoryBase<AuditLogEntity>, IAuditLogRepository
{
    public AuditLogRepository(ApplicationDbContext context, ILogger<RepositoryBase<AuditLogEntity>> logger)
        : base(context, logger)
    {
    }

    public async Task<AuditLogEntity> CreateAsync(AuditLogEntity auditLog)
    {
        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
        return auditLog;
    }

    public async Task<IEnumerable<AuditLogEntity>> GetByUserIdAsync(string userId, int skip = 0, int take = 50)
    {
        return await _context.AuditLogs
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IEnumerable<AuditLogEntity>> GetByActionAsync(string action, int skip = 0, int take = 50)
    {
        return await _context.AuditLogs
            .Where(a => a.Action == action)
            .OrderByDescending(a => a.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IEnumerable<AuditLogEntity>> GetByEntityAsync(string entityName, string? entityId = null, int skip = 0, int take = 50)
    {
        var query = _context.AuditLogs
            .Where(a => a.EntityName == entityName);

        if (!string.IsNullOrEmpty(entityId))
        {
            query = query.Where(a => a.EntityId == entityId);
        }

        return await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IEnumerable<AuditLogEntity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, int skip = 0, int take = 50)
    {
        return await _context.AuditLogs
            .Where(a => a.CreatedAt >= startDate && a.CreatedAt <= endDate)
            .OrderByDescending(a => a.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
}
