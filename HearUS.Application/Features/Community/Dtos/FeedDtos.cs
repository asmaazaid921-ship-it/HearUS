namespace HearUS.Application.Features.Community.Dtos;

public sealed class FeedResultDto
{
    public IReadOnlyList<FeedItemDto> Items { get; init; } = Array.Empty<FeedItemDto>();

    public int TotalCount { get; init; }

    public int Page { get; init; }

    public int PageSize { get; init; }

    public bool IsComposerVisible { get; init; }
}

public sealed class FeedItemDto
{
    public int Id { get; init; }

    public string AuthorName { get; init; } = string.Empty;

    public bool IsExpertAuthor { get; init; }

    public string Content { get; init; } = string.Empty;

    public string? ImageUrl { get; init; }

    public string? SignVideoUrl { get; init; }

    public DateTime CreatedAt { get; init; }

    public int LikesCount { get; init; }

    public bool IsLikedByCurrentUser { get; init; }

    public bool CanDelete { get; init; }

    public IReadOnlyList<FeedCommentDto> Comments { get; init; } = Array.Empty<FeedCommentDto>();
}

public sealed class FeedCommentDto
{
    public int Id { get; init; }

    public string AuthorName { get; init; } = string.Empty;

    public string Content { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }
}

public sealed record ToggleLikeResultDto(bool Liked, int Count);
