using HearUS.Domain.Entities;

namespace HearUS.Application.Features.Signs.Dtos;

public sealed class SignTermSummaryDto
{
    public int Id { get; init; }

    public string Term { get; init; } = string.Empty;

    public string? Description { get; init; }

    public SignCategory Category { get; init; }

    public DateTime CreatedAt { get; init; }
}

public sealed class SignTermDetailsDto
{
    public int Id { get; init; }

    public string Term { get; init; } = string.Empty;

    public string? Description { get; init; }

    public SignCategory Category { get; init; }

    public string VideoUrl { get; init; } = string.Empty;

    public string AuthorName { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; init; }
}

public sealed class SignTermEditDto
{
    public int Id { get; init; }

    public string Term { get; init; } = string.Empty;

    public string? Description { get; init; }

    public SignCategory Category { get; init; }

    public string VideoUrl { get; init; } = string.Empty;
}

public sealed class SignListResult
{
    public IReadOnlyList<SignTermSummaryDto> Items { get; init; } = Array.Empty<SignTermSummaryDto>();

    public int TotalCount { get; init; }

    public int Page { get; init; }

    public int PageSize { get; init; }

    public bool CanManage { get; init; }
}
