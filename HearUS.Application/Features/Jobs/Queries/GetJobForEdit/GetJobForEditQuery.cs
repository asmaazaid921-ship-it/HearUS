using HearUS.Application.Features.Jobs.Dtos;
using MediatR;

namespace HearUS.Application.Features.Jobs.Queries.GetJobForEdit;

public sealed record GetJobForEditQuery(int Id) : IRequest<JobEditDto>;
