namespace Medical.Application.Exceptions;
public class BillItemNotFoundException : Exception
{
    public BillItemNotFoundException(string? message = null)
        : base(message ?? "Bill item not found.")
    {
    }
}
