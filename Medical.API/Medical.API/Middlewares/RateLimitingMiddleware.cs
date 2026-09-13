namespace Medical.API.Middlewares;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    private readonly IConfiguration _configuration;
    private static readonly Dictionary<string, List<DateTime>> _requestLog = new();

    public RateLimitingMiddleware(
        RequestDelegate next,
        ILogger<RateLimitingMiddleware> logger,
        IConfiguration configuration)
    {
        _next = next;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientId = GetClientId(context);
        var endpoint = context.Request.Path.Value ?? string.Empty;
        var key = $"{clientId}:{endpoint}";

        var maxRequests = _configuration.GetValue<int>("RateLimiting:MaxRequests", 100);
        var timeWindowMinutes = _configuration.GetValue<int>("RateLimiting:TimeWindowMinutes", 1);

        // Clean old requests
        CleanOldRequests(timeWindowMinutes);

        // Check rate limit
        if (_requestLog.ContainsKey(key))
        {
            var requests = _requestLog[key];
            if (requests.Count >= maxRequests)
            {
                _logger.LogWarning("Rate limit exceeded for client {ClientId} on endpoint {Endpoint}", clientId, endpoint);
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Too many requests. Please try again later.");
                return;
            }
        }

        // Log request
        if (!_requestLog.ContainsKey(key))
        {
            _requestLog[key] = new List<DateTime>();
        }

        _requestLog[key].Add(DateTime.UtcNow);

        await _next(context);
    }

    private string GetClientId(HttpContext context)
    {
        // Try to get client ID from various sources
        var clientId = context.Request.Headers["X-Client-ID"].FirstOrDefault();
        if (!string.IsNullOrEmpty(clientId))
            return clientId;

        clientId = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(clientId))
            return clientId;

        clientId = context.Connection.RemoteIpAddress?.ToString();
        return clientId ?? "unknown";
    }

    private void CleanOldRequests(int timeWindowMinutes)
    {
        var cutoffTime = DateTime.UtcNow.AddMinutes(-timeWindowMinutes);
        var keysToRemove = new List<string>();

        foreach (var kvp in _requestLog)
        {
            kvp.Value.RemoveAll(x => x < cutoffTime);
            if (kvp.Value.Count == 0)
            {
                keysToRemove.Add(kvp.Key);
            }
        }

        foreach (var key in keysToRemove)
        {
            _requestLog.Remove(key);
        }
    }
}
