using System.ComponentModel.DataAnnotations;

namespace HearUS.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        public string AuthorId { get; set; } = string.Empty;

        public ApplicationUser Author { get; set; } = null!;

        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        [StringLength(500)]
        public string? SignVideoUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
    }
}
