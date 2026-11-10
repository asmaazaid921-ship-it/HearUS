using HearUS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HearUS.Infrastructure.Persistence.Configurations
{
    public class InsightConfiguration : IEntityTypeConfiguration<Insight>
    {
        public void Configure(EntityTypeBuilder<Insight> builder)
        {
            builder.Property(i => i.Category)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.HasIndex(i => new { i.IsPublished, i.CreatedAt });

            builder.HasOne(i => i.Author)
                .WithMany()
                .HasForeignKey(i => i.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
