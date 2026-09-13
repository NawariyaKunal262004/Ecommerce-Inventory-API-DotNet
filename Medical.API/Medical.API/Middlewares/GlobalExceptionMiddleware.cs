namespace Medical.API.Middlewares;
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Log the full exception details
        _logger.LogError(exception, "An unhandled exception occurred.");

        // Prepare the response
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // Create a detailed error response
        var errorResponse = new ErrorResponse
        {
            RequestId = Activity.Current?.Id ?? context.TraceIdentifier,
            Message = "An unexpected error occurred.",
            DetailedMessage = exception.Message,
            StackTrace = exception.StackTrace
        };

        switch (exception)
        {
            case InvalidCredentialsException:
                errorResponse.Message = exception.Message;
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;

            case AuthUnauthorizedException:
            case UnauthorizedAccessException:
                errorResponse.Message = exception.Message;
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                break;

            case UserNotFoundException:
                errorResponse.Message = exception.Message;
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;

            case RoleAssignmentException roleEx:
                errorResponse.Message = roleEx.Message;
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            case ValidationException validationEx:
                errorResponse.Message = "Validation failed.";
                errorResponse.DetailedMessage = string.Join("; ",
                    validationEx.Errors.Select(e => e.ErrorMessage));
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            case DbUpdateException dbUpdateEx:
                // Detailed logging for database update conflicts
                errorResponse.Message = "Database update error occurred.";
                errorResponse.DetailedMessage = GetDbUpdateExceptionDetails(dbUpdateEx);
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                break;

            case InvalidOperationException invalidOpEx when invalidOpEx.Message.Contains("cannot be tracked"):
                // Specific handling for entity tracking conflicts
                errorResponse.Message = "Entity tracking conflict detected.";
                errorResponse.DetailedMessage = "Multiple instances of the same entity are being tracked. " +
                    "Ensure only one instance of an entity with a given key is tracked at a time.";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
        }

        // Serialize and write the error response
        var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true 
        });
        await context.Response.WriteAsync(result);
    }

    private string GetDbUpdateExceptionDetails(DbUpdateException ex)
    {
        var innerException = ex.InnerException?.Message ?? "No inner exception details";
        var entityEntries = ex.Entries?.Select(e => 
            $"Entity: {e.Entity.GetType().Name}, State: {e.State}").ToList();

        return $"Database Update Error: {innerException}\n" +
               $"Affected Entities:\n{string.Join("\n", entityEntries ?? new List<string>())}";
    }
}

// Detailed error response model
public class ErrorResponse
{
    public string RequestId { get; set; }
    public string Message { get; set; }
    public string DetailedMessage { get; set; }
    public string StackTrace { get; set; }
}
