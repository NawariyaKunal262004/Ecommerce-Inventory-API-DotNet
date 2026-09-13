namespace Medical.Application.Exceptions;
public class MedicineBatchNotFoundException : Exception
{
    public MedicineBatchNotFoundException(string? message = null)
        : base(message ?? "Medicine batch not found.")
    {
    }
}
