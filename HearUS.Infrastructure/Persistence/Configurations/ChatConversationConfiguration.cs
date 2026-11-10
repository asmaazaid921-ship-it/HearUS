using HearUS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HearUS.Infrastructure.Persistence.Configurations
{
    public class ChatConversationConfiguration : IEntityTypeConfiguration<ChatConversation>
    {
        public void Configure(EntityTypeBuilder<ChatConversation> builder)
        {
            builder.HasOne(c => c.ParticipantOne)
                .WithMany()
                .HasForeignKey(c => c.ParticipantOneId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.ParticipantTwo)
                .WithMany()
                .HasForeignKey(c => c.ParticipantTwoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => new { c.ParticipantOneId, c.ParticipantTwoId }).IsUnique();
            builder.HasIndex(c => c.LastMessageAt);
        }
    }
}
