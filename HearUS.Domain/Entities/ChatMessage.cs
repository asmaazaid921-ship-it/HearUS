using System.ComponentModel.DataAnnotations;

namespace HearUS.Domain.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }

        public int ConversationId { get; set; }

        public ChatConversation Conversation { get; set; } = null!;

        [Required]
        public string SenderId { get; set; } = string.Empty;

        public ApplicationUser Sender { get; set; } = null!;

        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; }
    }
}
