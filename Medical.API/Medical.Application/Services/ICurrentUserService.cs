namespace Medical.Application.Services;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    Guid? OrganizationId { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
}
