using System.Security.Cryptography;

namespace Medical.Infra.Repositories;

public class UserRepository : RepositoryBase<ApplicationUser>, IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public UserRepository(
        ApplicationDbContext context,
        ILogger<RepositoryBase<ApplicationUser>> logger,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        IRefreshTokenRepository refreshTokenRepository) : base(context, logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<ResponseModel> CreateAsync(ApplicationUser user, string password, List<Claim>? claims = null)
    {
        var responseModel = new ResponseModel();
        var existingUser = await _userManager.FindByEmailAsync(user.Email!);
        if (existingUser != null)
        {
            responseModel.Success = false;
            responseModel.Message = CommonResource.RecordAlreadyExists;
            return responseModel;
        }

        if (!AuthConstants.ValidRoles.Contains(user.Role ?? string.Empty))
        {
            responseModel.Success = false;
            responseModel.Message = CommonResource.InvalidRole;
            return responseModel;
        }

        user.Pwd = null;
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            responseModel.Success = false;
            responseModel.Message = string.Join(", ", result.Errors.Select(e => e.Description));
            return responseModel;
        }

        await EnsureRoleAndAssignAsync(user);
        if (claims != null && claims.Count > 0)
            await _userManager.AddClaimsAsync(user, claims);

        await _context.SaveChangesAsync();

        responseModel.Success = true;
        responseModel.Message = CommonResource.RecordSavedSuccessfully;
        responseModel.Data = MapUser(user, user.Role);
        return responseModel;
    }

    public async Task<ResponseModel> GetUsersAsync(Guid organizationId)
    {
        var users = await _userManager.Users
            .Where(u => u.OrganizationId == organizationId)
            .ToListAsync();

        var result = new List<UserDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add(MapUser(user, roles.FirstOrDefault() ?? user.Role));
        }

        return new ResponseModel
        {
            Success = true,
            Data = result
        };
    }

    public async Task<ResponseModel> GetUserByIdAsync(string userId)
    {
        var responseModel = new ResponseModel { Success = true };
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            responseModel.Success = false;
            responseModel.Message = CommonResource.UserNotFound;
            return responseModel;
        }

        var roles = await _userManager.GetRolesAsync(user);
        responseModel.Data = MapUser(user, roles.FirstOrDefault() ?? user.Role);
        return responseModel;
    }

    public async Task<ResponseModel> LoginAsync(string userName, string password)
    {
        var responseModel = new ResponseModel();
        var user = await _userManager.FindByNameAsync(userName)
            ?? await _userManager.FindByEmailAsync(userName);

        if (user == null)
        {
            responseModel.Success = false;
            responseModel.Message = CommonResource.UserNotFound;
            return responseModel;
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
        {
            responseModel.Success = false;
            responseModel.Message = CommonResource.InvalidUsernameorPassword;
            return responseModel;
        }

        var roles = await _userManager.GetRolesAsync(user);
        var primaryRole = roles.FirstOrDefault() ?? user.Role ?? string.Empty;
        user.Role = primaryRole;

        var expirationMinutes = int.TryParse(
            _configuration[CommonFields.JwtColonExpirationMinutes], out var minutes)
            ? minutes
            : 60;
        var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var jwtToken = GenerateJwtToken(user, roles, expiration);
        var refreshToken = GenerateRefreshToken(jwtToken);

        // Save refresh token to database
        await _refreshTokenRepository.CreateAsync(refreshToken);

        var loginResult = new LoginResult
        {
            Token = jwtToken,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpiration = refreshToken.ExpiresAt,
            Expiration = expiration,
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Roles = roles.ToList(),
            OrganizationId = user.OrganizationId
        };

        responseModel.Success = true;
        responseModel.Message = CommonResource.UserLoggedInSuccessfully;
        responseModel.Data = loginResult;
        responseModel.RoleName = primaryRole;
        responseModel.OrganizationId = user.OrganizationId;
        return responseModel;
    }

    public new async Task<ResponseModel> UpdateAsync(ApplicationUser user)
    {
        var responseModel = new ResponseModel();
        var existingUser = await _userManager.FindByIdAsync(user.Id);
        if (existingUser == null)
        {
            responseModel.Success = false;
            responseModel.Message = CommonResource.UserNotFound;
            return responseModel;
        }

        if (!string.IsNullOrWhiteSpace(user.Role) && !AuthConstants.ValidRoles.Contains(user.Role))
        {
            responseModel.Success = false;
            responseModel.Message = CommonResource.InvalidRole;
            return responseModel;
        }

        existingUser.Email = user.Email;
        existingUser.UserName = user.UserName;
        existingUser.PhoneNumber = user.PhoneNumber;
        existingUser.OrganizationId = user.OrganizationId;

        var result = await _userManager.UpdateAsync(existingUser);
        if (!result.Succeeded)
        {
            responseModel.Success = false;
            responseModel.Message = string.Join(", ", result.Errors.Select(e => e.Description));
            return responseModel;
        }

        if (!string.IsNullOrWhiteSpace(user.Role))
        {
            var currentRoles = await _userManager.GetRolesAsync(existingUser);
            if (currentRoles.Count > 0)
                await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);

            existingUser.Role = user.Role;
            await EnsureRoleAndAssignAsync(existingUser);
            await _userManager.UpdateAsync(existingUser);
        }

        await _context.SaveChangesAsync();

        var roles = await _userManager.GetRolesAsync(existingUser);
        responseModel.Success = true;
        responseModel.Message = CommonResource.RecordSavedSuccessfully;
        responseModel.Data = MapUser(existingUser, roles.FirstOrDefault() ?? existingUser.Role);
        return responseModel;
    }

    public async Task<ResponseModel> DeleteAsync(string userId)
    {
        var responseModel = new ResponseModel();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            responseModel.Success = false;
            responseModel.Message = CommonResource.UserNotFound;
            return responseModel;
        }

        var result = await _userManager.DeleteAsync(user);
        responseModel.Success = result.Succeeded;
        responseModel.Message = result.Succeeded
            ? CommonResource.UserDeletedSuccessfully
            : string.Join(", ", result.Errors.Select(e => e.Description));
        return responseModel;
    }

    public async Task<ResponseModel> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var responseModel = new ResponseModel();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            responseModel.Success = false;
            responseModel.Message = CommonResource.UserNotFound;
            return responseModel;
        }

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        responseModel.Success = result.Succeeded;
        responseModel.Message = result.Succeeded
            ? CommonResource.PasswordChangedSuccessfully
            : string.Join(", ", result.Errors.Select(e => e.Description));
        return responseModel;
    }

    public async Task<ResponseModel> ResetPasswordAsync(string userId, string newPassword)
    {
        var responseModel = new ResponseModel();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            responseModel.Success = false;
            responseModel.Message = CommonResource.UserNotFound;
            return responseModel;
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        responseModel.Success = result.Succeeded;
        responseModel.Message = result.Succeeded
            ? CommonResource.PasswordResetSuccessfully
            : string.Join(", ", result.Errors.Select(e => e.Description));
        return responseModel;
    }

    public UserManager<ApplicationUser>? GetUserManager()
    {
        return _userManager;
    }

    private async Task EnsureRoleAndAssignAsync(ApplicationUser user)
    {
        var roleName = user.Role!;
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            var newRole = new ApplicationRole
            {
                Name = roleName,
                OrganizationId = user.OrganizationId
            };
            await _roleManager.CreateAsync(newRole);
        }

        if (!await _userManager.IsInRoleAsync(user, roleName))
            await _userManager.AddToRoleAsync(user, roleName);
    }

    private string GenerateJwtToken(ApplicationUser user, IList<string> roles, DateTime expiration)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")
                ?? throw new InvalidOperationException("JWT_KEY environment variable is required.")));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(CommonFields.UserId, user.Id),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(CommonFields.Username, user.UserName ?? string.Empty),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(CommonFields.OrganizationId, user.OrganizationId.ToString())
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        if (roles.Count == 0 && !string.IsNullOrWhiteSpace(user.Role))
            claims.Add(new Claim(ClaimTypes.Role, user.Role));

        var token = new JwtSecurityToken(
            _configuration[CommonFields.JwtColonIssuer],
            _configuration[CommonFields.JwtColonAudience],
            claims,
            expires: expiration,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private RefreshTokenEntity GenerateRefreshToken(string jwtToken)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var refreshToken = Convert.ToBase64String(randomNumber);

        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtTokenObj = jwtHandler.ReadJwtToken(jwtToken);
        var jwtId = string.IsNullOrWhiteSpace(jwtTokenObj.Id) ? Guid.NewGuid().ToString() : jwtTokenObj.Id;

        var refreshTokenExpirationDays = int.TryParse(
            _configuration["Jwt:RefreshTokenExpirationDays"], out var days)
            ? days
            : 7;

        return new RefreshTokenEntity
        {
            Token = refreshToken,
            JwtId = jwtId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationDays)
        };
    }

    private static UserDto MapUser(ApplicationUser user, string? role) => new()
    {
        Id = user.Id,
        UserName = user.UserName,
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        Role = role,
        OrganizationId = user.OrganizationId
    };
}
