using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Admin.Dtos;
using HearUS.Application.Features.Jobs.Common;
using MediatR;

namespace HearUS.Application.Features.Admin.Queries.GetAdminDashboard;

public sealed class GetAdminDashboardQueryHandler : IRequestHandler<GetAdminDashboardQuery, AdminDashboardDto>
{
    private readonly IIdentityService _identity;
    private readonly IPostRepository _posts;
    private readonly IJobRepository _jobs;
    private readonly IInsightRepository _insights;

    public GetAdminDashboardQueryHandler(
        IIdentityService identity,
        IPostRepository posts,
        IJobRepository jobs,
        IInsightRepository insights)
    {
        _identity = identity;
        _posts = posts;
        _jobs = jobs;
        _insights = insights;
    }

    public async Task<AdminDashboardDto> Handle(GetAdminDashboardQuery request, CancellationToken cancellationToken)
    {
        var (latestJobs, _) = await _jobs.GetAllAsync(1, 5);
        var (_, postsCount) = await _posts.GetFeedAsync(1, 1);
        var (_, activeJobsCount) = await _jobs.GetActiveAsync(1, 1);
        var (_, publishedInsightsCount) = await _insights.GetPublishedAsync(1, 1);

        return new AdminDashboardDto
        {
            UsersCount = await _identity.CountUsersAsync(),
            PostsCount = postsCount,
            PublishedInsightsCount = publishedInsightsCount,
            ActiveJobsCount = activeJobsCount,
            LatestJobs = latestJobs.Select(JobMapping.ToSummary).ToList()
        };
    }
}
