using HearUS.Application.Common.Identity;

namespace HearUS.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<RegisterUserResult> RegisterAsync(string fullName, string email, string? city, string password);

    Task<SignInAttemptResult> PasswordSignInAsync(string email, string password, bool rememberMe);

    Task SignOutAsync();

    Task<UserAccountDto?> GetUserAsync(string userId);

    Task<IReadOnlyList<string>> GetRolesAsync(string userId);

    Task<int> CountUsersAsync();
}
