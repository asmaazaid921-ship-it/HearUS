using HearUS.Application.Features.Chat.Dtos;

namespace HearUS.Areas.Chat.ViewModels
{
    public class ChatIndexViewModel
    {
        public IReadOnlyList<ChatConversationSummaryDto> Items { get; set; } = Array.Empty<ChatConversationSummaryDto>();
    }

    public class NewChatViewModel
    {
        public string? Search { get; set; }

        public IReadOnlyList<ChatUserDto> Users { get; set; } = Array.Empty<ChatUserDto>();
    }

    public class ConversationViewModel
    {
        public int ConversationId { get; set; }

        public string OtherName { get; set; } = string.Empty;

        public string OtherInitial { get; set; } = string.Empty;

        public string CurrentUserId { get; set; } = string.Empty;

        public IReadOnlyList<ChatMessageViewDto> Messages { get; set; } = Array.Empty<ChatMessageViewDto>();
    }
}
