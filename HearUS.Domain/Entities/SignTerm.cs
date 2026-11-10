using System.ComponentModel.DataAnnotations;

namespace HearUS.Domain.Entities
{
    public enum SignCategory
    {
        Alphabet = 0,
        Numbers = 1,
        DailyLife = 2,
        Health = 3,
        Work = 4,
        Emotions = 5,
        Other = 6
    }

    public class SignTerm
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Term { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public SignCategory Category { get; set; }

        [Required]
        [StringLength(500)]
        public string VideoUrl { get; set; } = string.Empty;

        [Required]
        public string CreatedById { get; set; } = string.Empty;

        public ApplicationUser CreatedBy { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
