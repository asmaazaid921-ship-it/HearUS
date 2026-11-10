namespace HearUS.Application.Features.Jobs.Dtos;

public sealed class JobSummaryDto
{
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string CompanyName { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string? Location { get; init; }

    public bool IsRemote { get; init; }

    public bool IsAccessibilityFriendly { get; init; }

    public string? SignVideoUrl { get; init; }

    public DateTime PostedAt { get; init; }

    public bool IsActive { get; init; }
}

public sealed class JobDetailsDto
{
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string CompanyName { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string? Location { get; init; }

    public bool IsRemote { get; init; }

    public bool IsAccessibilityFriendly { get; init; }

    public string? SignVideoUrl { get; init; }

    public string? ApplyUrl { get; init; }

    public string? ContactEmail { get; init; }

    public DateTime PostedAt { get; init; }

    public bool IsActive { get; init; }
}

public sealed class JobEditDto
{
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string CompanyName { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string? SignVideoUrl { get; init; }

    public string? Location { get; init; }

    public bool IsRemote { get; init; }

    public bool IsAccessibilityFriendly { get; init; }

    public string? ApplyUrl { get; init; }

    public string? ContactEmail { get; init; }

    public bool IsActive { get; init; }
}

public sealed class JobListResult
{
    public IReadOnlyList<JobSummaryDto> Items { get; init; } = Array.Empty<JobSummaryDto>();

    public int TotalCount { get; init; }

    public int Page { get; init; }

    public int PageSize { get; init; }
}
