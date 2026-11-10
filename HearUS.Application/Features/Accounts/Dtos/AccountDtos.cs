namespace HearUS.Application.Features.Accounts.Dtos;

public sealed class UserProfileDto
{
    public string FullName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string? City { get; init; }

    public string? Bio { get; init; }

    public DateTime CreatedAt { get; init; }

    public IReadOnlyList<string> Roles { get; init; } = Array.Empty<string>();

    public int PostsCount { get; init; }

    public int InsightsCount { get; init; }
}
