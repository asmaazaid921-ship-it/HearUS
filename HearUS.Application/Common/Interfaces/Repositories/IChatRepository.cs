using HearUS.Domain.Entities;

namespace HearUS.Application.Common.Interfaces.Repositories
{
    public interface IChatRepository
    {
        Task<ChatConversation?> GetConversationAsync(int conversationId);

        Task<ChatConversation> GetOrCreateBetweenAsync(string participantOneId, string participantTwoId);

        Task<IReadOnlyList<ChatConversation>> GetConversationsForUserAsync(string userId);

        Task<IReadOnlyList<ChatMessage>> GetMessagesAsync(int conversationId, int take = 50);

        Task AddConversationAsync(ChatConversation conversation);

        Task AddMessageAsync(ChatMessage message);

        Task<int> MarkReadAsync(int conversationId, string userId);

        Task<int> CountUnreadAsync(string userId);
    }
}
