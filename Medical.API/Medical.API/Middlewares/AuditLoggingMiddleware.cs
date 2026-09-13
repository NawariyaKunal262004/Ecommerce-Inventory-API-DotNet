namespace Medical.API.Middlewares;

public class AuditLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuditLoggingMiddleware> _logger;

    public AuditLoggingMiddleware(RequestDelegate next, ILogger<AuditLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip logging for static files and health checks
        if (context.Request.Path.StartsWithSegments("/static") || 
            context.Request.Path.StartsWithSegments("/health"))
        {
            await _next(context);
            return;
        }

        var originalBodyStream = context.Response.Body;
        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        await _next(context);

        memoryStream.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(memoryStream).ReadToEnd();
        memoryStream.Seek(0, SeekOrigin.Begin);
        await memoryStream.CopyToAsync(originalBodyStream);

        // Log the request
        _logger.LogInformation("Request: {Method} {Path} | Status: {StatusCode} | User: {UserId}",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            context.User?.Identity?.Name ?? "Anonymous");
    }
}
