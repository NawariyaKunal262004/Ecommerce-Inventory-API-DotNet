namespace Medical.Application.Exceptions;

public class RoleAssignmentException : Exception
{
    public RoleAssignmentException(string? message = null)
        : base(message ?? "Invalid role assignment.")
    {
    }
}
