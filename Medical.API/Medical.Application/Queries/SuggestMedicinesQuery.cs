namespace Medical.Application.Queries;
public class SuggestMedicinesQuery : IRequest<List<MedicineSuggestionResponse>>
{
    public Guid? PatientId { get; set; }
}
