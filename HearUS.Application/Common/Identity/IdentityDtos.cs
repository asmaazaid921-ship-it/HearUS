namespace HearUS.Application.Common.Identity;

public sealed record IdentityErrorDto(string Code, string Description);

public sealed record RegisterUserResult(
    bool Succeeded,
    IReadOnlyList<IdentityErrorDto> Errors);

public sealed record SignInAttemptResult(bool Succeeded, bool IsLockedOut);

public sealed record UserAccountDto(
    string Id,
    string FullName,
    string? Email,
    string? City,
    string? Bio,
    DateTime CreatedAt);
