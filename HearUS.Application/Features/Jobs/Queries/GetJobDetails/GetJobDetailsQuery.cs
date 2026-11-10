using HearUS.Application.Features.Jobs.Dtos;
using MediatR;

namespace HearUS.Application.Features.Jobs.Queries.GetJobDetails;

public sealed record GetJobDetailsQuery(int Id) : IRequest<JobDetailsDto>;
