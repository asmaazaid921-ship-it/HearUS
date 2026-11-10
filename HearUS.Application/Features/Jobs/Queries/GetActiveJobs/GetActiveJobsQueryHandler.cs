using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Jobs.Common;
using HearUS.Application.Features.Jobs.Dtos;
using MediatR;

namespace HearUS.Application.Features.Jobs.Queries.GetActiveJobs;

public sealed class GetActiveJobsQueryHandler : IRequestHandler<GetActiveJobsQuery, JobListResult>
{
    private const int PageSize = 9;

    private readonly IJobRepository _jobs;

    public GetActiveJobsQueryHandler(IJobRepository jobs)
    {
        _jobs = jobs;
    }

    public async Task<JobListResult> Handle(GetActiveJobsQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var (items, totalCount) = await _jobs.GetActiveAsync(page, PageSize, request.Search);

        return new JobListResult
        {
            TotalCount = totalCount,
            Page = page,
            PageSize = PageSize,
            Items = items.Select(JobMapping.ToSummary).ToList()
        };
    }
}
