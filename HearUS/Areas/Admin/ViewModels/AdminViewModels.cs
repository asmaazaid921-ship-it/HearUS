using System.ComponentModel.DataAnnotations;
using HearUS.Application.Features.Jobs.Dtos;
using HearUS.Validation;
using HearUS.ViewModels;

namespace HearUS.Areas.Admin.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int UsersCount { get; set; }

        public int PostsCount { get; set; }

        public int PublishedInsightsCount { get; set; }

        public int ActiveJobsCount { get; set; }

        public IReadOnlyList<JobSummaryDto> LatestJobs { get; set; } = Array.Empty<JobSummaryDto>();
    }

    public class AdminJobsViewModel
    {
        public IReadOnlyList<JobSummaryDto> Items { get; set; } = Array.Empty<JobSummaryDto>();

        public string? Search { get; set; }

        public PagerViewModel Pager { get; set; } = new();
    }

    public class JobFormViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "عنوان الوظيفة مطلوب")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "العنوان يجب أن يكون بين 3 و150 حرفاً")]
        [Display(Name = "عنوان الوظيفة")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم الشركة مطلوب")]
        [StringLength(100, ErrorMessage = "اسم الشركة يجب ألا يتجاوز 100 حرف")]
        [Display(Name = "اسم الشركة")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "وصف الوظيفة مطلوب")]
        [StringLength(4000, MinimumLength = 20, ErrorMessage = "الوصف يجب أن يكون بين 20 و4000 حرف")]
        [Display(Name = "وصف الوظيفة")]
        public string Description { get; set; } = string.Empty;

        [SignVideoUrl]
        [StringLength(500, ErrorMessage = "رابط الفيديو يجب ألا يتجاوز 500 حرف")]
        [Display(Name = "رابط فيديو بلغة الإشارة (اختياري)")]
        public string? SignVideoUrl { get; set; }

        [StringLength(100, ErrorMessage = "الموقع يجب ألا يتجاوز 100 حرف")]
        [Display(Name = "الموقع")]
        public string? Location { get; set; }

        [Display(Name = "عمل عن بُعد")]
        public bool IsRemote { get; set; }

        [Display(Name = "مهيّأة لذوي الهمم")]
        public bool IsAccessibilityFriendly { get; set; }

        [StringLength(300)]
        [Url(ErrorMessage = "رابط التقديم غير صالح")]
        [Display(Name = "رابط التقديم")]
        public string? ApplyUrl { get; set; }

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
        [StringLength(150)]
        [Display(Name = "بريد جهة التوظيف")]
        public string? ContactEmail { get; set; }

        [Display(Name = "منشورة")]
        public bool IsActive { get; set; } = true;
    }
}
