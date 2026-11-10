using System.ComponentModel.DataAnnotations;

namespace HearUS.Domain.Entities
{
    public enum InsightCategory
    {
        Experience = 0,
        Awareness = 1,
        SuccessStory = 2,
        Guidance = 3
    }

    public class Insight
    {
        public int Id { get; set; }

        [Required]
        public string AuthorId { get; set; } = string.Empty;

        public ApplicationUser Author { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Summary { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [StringLength(500)]
        public string? SignVideoUrl { get; set; }

        public InsightCategory Category { get; set; }

        public bool IsPublished { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? PublishedAt { get; set; }
    }
}
