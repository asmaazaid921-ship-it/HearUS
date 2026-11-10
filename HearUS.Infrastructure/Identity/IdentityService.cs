using HearUS.Application.Common.Identity;
using HearUS.Application.Common.Interfaces;
using HearUS.Domain.Constants;
using HearUS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HearUS.Infrastructure.Identity;

public sealed class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<RegisterUserResult> RegisterAsync(string fullName, string email, string? city, string password)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = fullName,
            City = city
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return new RegisterUserResult(
                false,
                result.Errors.Select(error => new IdentityErrorDto(error.Code, error.Description)).ToList());
        }

        await _userManager.AddToRoleAsync(user, AppRoles.Member);
        await _signInManager.SignInAsync(user, isPersistent: false);

        return new RegisterUserResult(true, Array.Empty<IdentityErrorDto>());
    }

    public async Task<SignInAttemptResult> PasswordSignInAsync(string email, string password, bool rememberMe)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: true);

        return new SignInAttemptResult(result.Succeeded, result.IsLockedOut);
    }

    public Task SignOutAsync() => _signInManager.SignOutAsync();

    public async Task<UserAccountDto?> GetUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user is null
            ? null
            : new UserAccountDto(user.Id, user.FullName, user.Email, user.City, user.Bio, user.CreatedAt);
    }

    public async Task<IReadOnlyList<string>> GetRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user is null
            ? Array.Empty<string>()
            : (await _userManager.GetRolesAsync(user)).ToList();
    }

    public Task<int> CountUsersAsync() => _userManager.Users.CountAsync();
}
