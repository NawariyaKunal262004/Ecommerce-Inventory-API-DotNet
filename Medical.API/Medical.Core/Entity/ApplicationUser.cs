namespace Medical.Core.Entity;
public class ApplicationUser : IdentityUser
{
    public string? Pwd { get; set; }
    public string? Role { get; set; }
    public Guid OrganizationId { get; set; }
}
