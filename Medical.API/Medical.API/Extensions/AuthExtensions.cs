namespace Medical.API.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>() ?? new JwtSettings();
        var signingKey = Environment.GetEnvironmentVariable("JWT_KEY");

        if (string.IsNullOrWhiteSpace(signingKey))
            throw new InvalidOperationException("JWT_KEY environment variable is required.");

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = string.IsNullOrWhiteSpace(jwtSettings.Issuer) ? "Medical.API" : jwtSettings.Issuer,
                    ValidAudience = string.IsNullOrWhiteSpace(jwtSettings.Audience) ? "Medical.API.Client" : jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                    ClockSkew = TimeSpan.FromMinutes(2),
                    RoleClaimType = ClaimTypes.Role
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole(AuthConstants.Admin));
            options.AddPolicy("AdminOrBiller", policy => policy.RequireRole(AuthConstants.Admin, AuthConstants.Biller));
        });

        return services;
    }
}
