namespace HearUS.Domain.Entities
{
    public class ChatConversation
    {
        public int Id { get; set; }

        public string ParticipantOneId { get; set; } = string.Empty;

        public ApplicationUser ParticipantOne { get; set; } = null!;

        public string ParticipantTwoId { get; set; } = string.Empty;

        public ApplicationUser ParticipantTwo { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime LastMessageAt { get; set; } = DateTime.UtcNow;

        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}
