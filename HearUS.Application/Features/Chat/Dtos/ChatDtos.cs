namespace HearUS.Application.Features.Chat.Dtos;

public sealed class ChatConversationSummaryDto
{
    public int Id { get; init; }

    public string OtherName { get; init; } = string.Empty;

    public string OtherInitial { get; init; } = string.Empty;

    public string? LastMessage { get; init; }

    public DateTime LastActivityAt { get; init; }
}

public sealed class ChatUserDto
{
    public string Id { get; init; } = string.Empty;

    public string FullName { get; init; } = string.Empty;

    public string? Email { get; init; }
}

public sealed class ChatMessageViewDto
{
    public int Id { get; init; }

    public string SenderId { get; init; } = string.Empty;

    public string SenderName { get; init; } = string.Empty;

    public string Content { get; init; } = string.Empty;

    public DateTime SentAt { get; init; }
}

public sealed class ChatMessageDto
{
    public int Id { get; init; }

    public int ConversationId { get; init; }

    public string SenderId { get; init; } = string.Empty;

    public string SenderName { get; init; } = string.Empty;

    public string Content { get; init; } = string.Empty;

    public DateTime SentAt { get; init; }
}

public sealed class ChatConversationDto
{
    public int ConversationId { get; init; }

    public string OtherName { get; init; } = string.Empty;

    public string OtherInitial { get; init; } = string.Empty;

    public string CurrentUserId { get; init; } = string.Empty;

    public IReadOnlyList<ChatMessageViewDto> Messages { get; init; } = Array.Empty<ChatMessageViewDto>();
}

public sealed class ChatInboxDto
{
    public IReadOnlyList<ChatConversationSummaryDto> Items { get; init; } = Array.Empty<ChatConversationSummaryDto>();
}

public sealed class ChatUserSearchResult
{
    public string? Search { get; init; }

    public IReadOnlyList<ChatUserDto> Users { get; init; } = Array.Empty<ChatUserDto>();
}
