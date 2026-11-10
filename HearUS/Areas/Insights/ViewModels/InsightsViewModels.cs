using System.ComponentModel.DataAnnotations;
using HearUS.Application.Features.Insights.Dtos;
using HearUS.Domain.Entities;
using HearUS.Validation;
using HearUS.ViewModels;

namespace HearUS.Areas.Insights.ViewModels
{
    public class InsightsIndexViewModel
    {
        public IReadOnlyList<InsightSummaryDto> Items { get; set; } = Array.Empty<InsightSummaryDto>();

        public InsightCategory? Category { get; set; }

        public bool CanWrite { get; set; }

        public PagerViewModel Pager { get; set; } = new();
    }

    public class InsightFormViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "العنوان مطلوب")]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "العنوان يجب أن يكون بين 5 و150 حرفاً")]
        [Display(Name = "العنوان")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "الملخص يجب ألا يتجاوز 500 حرف")]
        [Display(Name = "ملخص قصير")]
        public string? Summary { get; set; }

        [Required(ErrorMessage = "نص المقال مطلوب")]
        [Display(Name = "نص المقال")]
        public string Content { get; set; } = string.Empty;

        [SignVideoUrl]
        [StringLength(500, ErrorMessage = "رابط الفيديو يجب ألا يتجاوز 500 حرف")]
        [Display(Name = "رابط فيديو بلغة الإشارة (اختياري)")]
        public string? SignVideoUrl { get; set; }

        [Display(Name = "التصنيف")]
        public InsightCategory Category { get; set; }

        [Display(Name = "نشر المقال مباشرة")]
        public bool IsPublished { get; set; } = true;
    }

    public class MyInsightsViewModel
    {
        public IReadOnlyList<InsightSummaryDto> Items { get; set; } = Array.Empty<InsightSummaryDto>();

        public PagerViewModel Pager { get; set; } = new();
    }
}
