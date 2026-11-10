using HearUS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HearUS.Infrastructure.Persistence.Configurations
{
    public class SignTermConfiguration : IEntityTypeConfiguration<SignTerm>
    {
        public void Configure(EntityTypeBuilder<SignTerm> builder)
        {
            builder.Property(t => t.Category)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.HasIndex(t => new { t.Category, t.Term });

            builder.Property(t => t.VideoUrl).HasMaxLength(500);

            builder.HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
