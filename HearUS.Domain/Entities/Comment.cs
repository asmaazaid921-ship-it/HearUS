using System.ComponentModel.DataAnnotations;

namespace HearUS.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; } = null!;

        [Required]
        public string AuthorId { get; set; } = string.Empty;

        public ApplicationUser Author { get; set; } = null!;

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
