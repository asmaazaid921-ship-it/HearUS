using HearUS.Application.Features.Jobs.Dtos;
using MediatR;

namespace HearUS.Application.Features.Jobs.Queries.GetActiveJobs;

public sealed record GetActiveJobsQuery(string? Search, int Page = 1) : IRequest<JobListResult>;
