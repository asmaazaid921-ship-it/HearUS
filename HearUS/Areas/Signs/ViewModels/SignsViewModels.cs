using System.ComponentModel.DataAnnotations;
using HearUS.Application.Features.Signs.Dtos;
using HearUS.Domain.Entities;
using HearUS.Validation;
using HearUS.ViewModels;

namespace HearUS.Areas.Signs.ViewModels
{
    public class SignsIndexViewModel
    {
        public IReadOnlyList<SignTermSummaryDto> Items { get; set; } = Array.Empty<SignTermSummaryDto>();

        public string? Search { get; set; }

        public SignCategory? Category { get; set; }

        public bool CanManage { get; set; }

        public PagerViewModel Pager { get; set; } = new();
    }

    public class SignFormViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "الكلمة أو العبارة مطلوبة")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "الكلمة يجب أن تكون بين 2 و100 حرف")]
        [Display(Name = "الكلمة أو العبارة")]
        public string Term { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "الوصف يجب ألا يتجاوز 500 حرف")]
        [Display(Name = "وصف قصير (اختياري)")]
        public string? Description { get; set; }

        [Display(Name = "التصنيف")]
        public SignCategory Category { get; set; }

        [Required(ErrorMessage = "رابط الفيديو مطلوب")]
        [SignVideoUrl]
        [StringLength(500, ErrorMessage = "رابط الفيديو يجب ألا يتجاوز 500 حرف")]
        [Display(Name = "رابط فيديو الإشارة")]
        public string VideoUrl { get; set; } = string.Empty;
    }
}
