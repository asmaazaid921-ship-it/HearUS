using HearUS.Application.Features.Jobs.Dtos;

namespace HearUS.Application.Features.Admin.Dtos;

public sealed class AdminDashboardDto
{
    public int UsersCount { get; init; }

    public int PostsCount { get; init; }

    public int PublishedInsightsCount { get; init; }

    public int ActiveJobsCount { get; init; }

    public IReadOnlyList<JobSummaryDto> LatestJobs { get; init; } = Array.Empty<JobSummaryDto>();
}
