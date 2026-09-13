namespace Medical.Application.Exceptions;
public class BillNotFoundException : Exception
{
    public BillNotFoundException(string? message = null)
        : base(message ?? "Bill not found.")
    {
    }
}
