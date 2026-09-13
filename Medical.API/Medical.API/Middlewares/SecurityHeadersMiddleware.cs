namespace Medical.API.Middlewares;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public SecurityHeadersMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Content Security Policy
        var csp = _configuration.GetValue<string>("SecurityHeaders:ContentSecurityPolicy") 
            ?? "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval'; style-src 'self' 'unsafe-inline'; img-src 'self' data: https:; font-src 'self' data:; connect-src 'self'; frame-ancestors 'none';";
        context.Response.Headers.Append("Content-Security-Policy", csp);

        // X-Frame-Options (Clickjacking protection)
        context.Response.Headers.Append("X-Frame-Options", "DENY");

        // X-Content-Type-Options (MIME sniffing protection)
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

        // X-XSS-Protection (XSS filter)
        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");

        // Referrer Policy
        context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");

        // Permissions Policy (formerly Feature Policy)
        var permissionsPolicy = _configuration.GetValue<string>("SecurityHeaders:PermissionsPolicy")
            ?? "geolocation=(), microphone=(), camera=()";
        context.Response.Headers.Append("Permissions-Policy", permissionsPolicy);

        // Strict-Transport-Security (HSTS) - Only in production with HTTPS
        if (context.Request.IsHttps && !_configuration.GetValue<bool>("SecurityHeaders:DisableHSTS"))
        {
            var hstsMaxAge = _configuration.GetValue<int>("SecurityHeaders:HSTSMaxAge", 31536000); // 1 year
            context.Response.Headers.Append("Strict-Transport-Security", $"max-age={hstsMaxAge}; includeSubDomains; preload");
        }

        // Remove server header
        context.Response.Headers.Remove("Server");

        // X-Powered-By header removal
        context.Response.Headers.Remove("X-Powered-By");

        await _next(context);
    }
}
