namespace Medical.Application.Handlers;
public class SuggestMedicinesQueryHandler : IRequestHandler<SuggestMedicinesQuery, List<MedicineSuggestionResponse>>
{
    private readonly IAIService _aiService;

    public SuggestMedicinesQueryHandler(IAIService aiService)
    {
        _aiService = aiService;
    }

    public async Task<List<MedicineSuggestionResponse>> Handle(SuggestMedicinesQuery request, CancellationToken cancellationToken)
    {
        return await _aiService.SuggestMedicinesAsync(request.PatientId, cancellationToken);
    }
}
