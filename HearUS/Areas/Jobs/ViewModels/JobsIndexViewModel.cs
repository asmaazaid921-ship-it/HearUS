using HearUS.Application.Features.Jobs.Dtos;
using HearUS.ViewModels;

namespace HearUS.Areas.Jobs.ViewModels
{
    public class JobsIndexViewModel
    {
        public IReadOnlyList<JobSummaryDto> Items { get; set; } = Array.Empty<JobSummaryDto>();

        public string? Search { get; set; }

        public PagerViewModel Pager { get; set; } = new();
    }
}
