using HearUS.Infrastructure.Persistence;
using HearUS.Domain.Entities;
using HearUS.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HearUS.Infrastructure.Persistence.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<ChatConversation?> GetConversationAsync(int conversationId)
        {
            return _context.ChatConversations
                .Include(c => c.ParticipantOne)
                .Include(c => c.ParticipantTwo)
                .FirstOrDefaultAsync(c => c.Id == conversationId);
        }

        public async Task<ChatConversation> GetOrCreateBetweenAsync(string participantOneId, string participantTwoId)
        {
            var existing = await FindBetweenAsync(participantOneId, participantTwoId);
            if (existing is not null)
            {
                return existing;
            }

            var conversation = new ChatConversation
            {
                ParticipantOneId = participantOneId,
                ParticipantTwoId = participantTwoId,
                CreatedAt = DateTime.UtcNow,
                LastMessageAt = DateTime.UtcNow
            };

            await _context.ChatConversations.AddAsync(conversation);

            try
            {
                await _context.SaveChangesAsync();
                return conversation;
            }
            catch (DbUpdateException)
            {
                _context.Entry(conversation).State = EntityState.Detached;
                var raced = await FindBetweenAsync(participantOneId, participantTwoId);
                if (raced is null)
                {
                    throw;
                }

                return raced;
            }
        }

        private Task<ChatConversation?> FindBetweenAsync(string userAId, string userBId)
        {
            return _context.ChatConversations
                .Include(c => c.ParticipantOne)
                .Include(c => c.ParticipantTwo)
                .FirstOrDefaultAsync(c =>
                    (c.ParticipantOneId == userAId && c.ParticipantTwoId == userBId) ||
                    (c.ParticipantOneId == userBId && c.ParticipantTwoId == userAId));
        }

        public async Task<IReadOnlyList<ChatConversation>> GetConversationsForUserAsync(string userId)
        {
            return await _context.ChatConversations
                .AsNoTracking()
                .Include(c => c.ParticipantOne)
                .Include(c => c.ParticipantTwo)
                .Include(c => c.Messages.OrderByDescending(m => m.SentAt).Take(1))
                .Where(c => c.ParticipantOneId == userId || c.ParticipantTwoId == userId)
                .OrderByDescending(c => c.LastMessageAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<ChatMessage>> GetMessagesAsync(int conversationId, int take = 50)
        {
            var messages = await _context.ChatMessages
                .AsNoTracking()
                .Include(m => m.Sender)
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.SentAt)
                .Take(Math.Clamp(take, 1, 200))
                .ToListAsync();

            messages.Reverse();
            return messages;
        }

        public Task AddConversationAsync(ChatConversation conversation)
        {
            return _context.ChatConversations.AddAsync(conversation).AsTask();
        }

        public Task AddMessageAsync(ChatMessage message)
        {
            return _context.ChatMessages.AddAsync(message).AsTask();
        }

        public Task<int> MarkReadAsync(int conversationId, string userId)
        {
            return _context.ChatMessages
                .Where(m => m.ConversationId == conversationId && m.SenderId != userId && !m.IsRead)
                .ExecuteUpdateAsync(setters => setters.SetProperty(m => m.IsRead, true));
        }

        public Task<int> CountUnreadAsync(string userId)
        {
            return _context.ChatMessages.CountAsync(m =>
                !m.IsRead &&
                m.SenderId != userId &&
                (m.Conversation.ParticipantOneId == userId || m.Conversation.ParticipantTwoId == userId));
        }
    }
}
