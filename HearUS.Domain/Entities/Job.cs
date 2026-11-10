using System.ComponentModel.DataAnnotations;

namespace HearUS.Domain.Entities
{
    public class Job
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;

        [StringLength(500)]
        public string? SignVideoUrl { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        public bool IsRemote { get; set; }

        public bool IsAccessibilityFriendly { get; set; }

        [StringLength(300)]
        public string? ApplyUrl { get; set; }

        [StringLength(150)]
        public string? ContactEmail { get; set; }

        public DateTime PostedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}
