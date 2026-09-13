namespace Medical.Core.Common;
public class PatientResponse
{
    public Guid PatientId { get; set; }

    public string? PatientName { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public int Age { get; set; }

    public char Gender { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public string? EmergencyContactName { get; set; }

    public string? EmergencyContactPhone { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastUpdatedAt { get; set; }
}
