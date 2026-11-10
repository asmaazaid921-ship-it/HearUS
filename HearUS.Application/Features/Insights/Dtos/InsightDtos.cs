using HearUS.Domain.Entities;

namespace HearUS.Application.Features.Insights.Dtos;

public sealed class InsightSummaryDto
{
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string? Summary { get; init; }

    public InsightCategory Category { get; init; }

    public string? SignVideoUrl { get; init; }

    public string AuthorName { get; init; } = string.Empty;

    public bool IsPublished { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime? PublishedAt { get; init; }
}

public sealed class InsightDetailsDto
{
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string? Summary { get; init; }

    public string Content { get; init; } = string.Empty;

    public InsightCategory Category { get; init; }

    public string? SignVideoUrl { get; init; }

    public string AuthorName { get; init; } = string.Empty;

    public bool IsPublished { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime? PublishedAt { get; init; }
}

public sealed class InsightEditDto
{
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string? Summary { get; init; }

    public string Content { get; init; } = string.Empty;

    public string? SignVideoUrl { get; init; }

    public InsightCategory Category { get; init; }

    public bool IsPublished { get; init; }
}

public sealed class InsightListResult
{
    public IReadOnlyList<InsightSummaryDto> Items { get; init; } = Array.Empty<InsightSummaryDto>();

    public int TotalCount { get; init; }

    public int Page { get; init; }

    public int PageSize { get; init; }

    public bool CanWrite { get; init; }
}

public sealed class MyInsightsResult
{
    public IReadOnlyList<InsightSummaryDto> Items { get; init; } = Array.Empty<InsightSummaryDto>();

    public int TotalCount { get; init; }

    public int Page { get; init; }

    public int PageSize { get; init; }
}
