namespace Medical.Core.IRepositories;

public interface IAuditLogRepository
{
    Task<AuditLogEntity> CreateAsync(AuditLogEntity auditLog);
    Task<IEnumerable<AuditLogEntity>> GetByUserIdAsync(string userId, int skip = 0, int take = 50);
    Task<IEnumerable<AuditLogEntity>> GetByActionAsync(string action, int skip = 0, int take = 50);
    Task<IEnumerable<AuditLogEntity>> GetByEntityAsync(string entityName, string? entityId = null, int skip = 0, int take = 50);
    Task<IEnumerable<AuditLogEntity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, int skip = 0, int take = 50);
}
