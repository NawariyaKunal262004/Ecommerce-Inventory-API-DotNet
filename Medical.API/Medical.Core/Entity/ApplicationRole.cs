namespace Medical.Core.Entity;
public class ApplicationRole : IdentityRole
{
    public required Guid OrganizationId { get; set; }
    public string? RoleName { get; set; }
}
