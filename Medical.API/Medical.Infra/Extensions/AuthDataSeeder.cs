namespace Medical.Infra.Extensions;

public static class AuthDataSeeder
{
    private static readonly Guid DefaultOrganizationId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public static async Task SeedAsync(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var roleName in AuthConstants.ValidRoles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = roleName,
                    OrganizationId = DefaultOrganizationId
                });
            }
        }

        if (!bool.TryParse(Environment.GetEnvironmentVariable("SEED_ADMIN_ENABLED"), out var seedEnabled)
            || !seedEnabled)
            return;

        var email = Environment.GetEnvironmentVariable("SEED_ADMIN_EMAIL");
        var password = Environment.GetEnvironmentVariable("SEED_ADMIN_PASSWORD");
        var userName = Environment.GetEnvironmentVariable("SEED_ADMIN_USERNAME") ?? "admin";
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return;

        if (await userManager.FindByEmailAsync(email) != null)
            return;

        var admin = new ApplicationUser
        {
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
            Role = AuthConstants.Admin,
            OrganizationId = DefaultOrganizationId
        };

        var result = await userManager.CreateAsync(admin, password);
        if (result.Succeeded)
            await userManager.AddToRoleAsync(admin, AuthConstants.Admin);
    }
}
