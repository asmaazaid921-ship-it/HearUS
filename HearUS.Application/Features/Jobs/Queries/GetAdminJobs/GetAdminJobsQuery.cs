using HearUS.Application.Features.Jobs.Dtos;
using MediatR;

namespace HearUS.Application.Features.Jobs.Queries.GetAdminJobs;

public sealed record GetAdminJobsQuery(string? Search, int Page = 1) : IRequest<JobListResult>;
