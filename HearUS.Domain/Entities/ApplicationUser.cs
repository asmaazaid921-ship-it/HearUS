using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HearUS.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? City { get; set; }

        [StringLength(500)]
        public string? Bio { get; set; }

        public string? AvatarUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}
