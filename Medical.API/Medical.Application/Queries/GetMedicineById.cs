namespace Medical.Application.Queries;

/// GetMedicineById is a CQRS Query that represents a request to retrieve a specific medicine by its ID.
/// It carries the ID parameter needed to fetch the specific record.

public class GetMedicineById : IRequest<ResponseModel>
{
    public GetMedicineById(Guid id) 
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
