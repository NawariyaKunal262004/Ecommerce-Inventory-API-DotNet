namespace Medical.Core.IRepositories;

public interface IUserRepository
{
    Task<ResponseModel> CreateAsync(ApplicationUser user, string password, List<Claim>? claims = null);
    Task<ResponseModel> GetUsersAsync(Guid organizationId);
    Task<ResponseModel> GetUserByIdAsync(string userId);
    Task<ResponseModel> LoginAsync(string userName, string password);
    Task<ResponseModel> UpdateAsync(ApplicationUser user);
    Task<ResponseModel> DeleteAsync(string userId);
    Task<ResponseModel> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    Task<ResponseModel> ResetPasswordAsync(string userId, string newPassword);
    UserManager<ApplicationUser>? GetUserManager();
}
