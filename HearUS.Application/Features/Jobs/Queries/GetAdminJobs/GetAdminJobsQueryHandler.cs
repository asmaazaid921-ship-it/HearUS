using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Jobs.Common;
using HearUS.Application.Features.Jobs.Dtos;
using MediatR;

namespace HearUS.Application.Features.Jobs.Queries.GetAdminJobs;

public sealed class GetAdminJobsQueryHandler : IRequestHandler<GetAdminJobsQuery, JobListResult>
{
    private const int PageSize = 15;

    private readonly IJobRepository _jobs;

    public GetAdminJobsQueryHandler(IJobRepository jobs)
    {
        _jobs = jobs;
    }

    public async Task<JobListResult> Handle(GetAdminJobsQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var (items, totalCount) = await _jobs.GetAllAsync(page, PageSize, request.Search);

        return new JobListResult
        {
            TotalCount = totalCount,
            Page = page,
            PageSize = PageSize,
            Items = items.Select(JobMapping.ToSummary).ToList()
        };
    }
}
