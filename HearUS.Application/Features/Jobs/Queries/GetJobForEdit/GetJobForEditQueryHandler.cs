using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Jobs.Dtos;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Jobs.Queries.GetJobForEdit;

public sealed class GetJobForEditQueryHandler : IRequestHandler<GetJobForEditQuery, JobEditDto>
{
    private readonly IJobRepository _jobs;

    public GetJobForEditQueryHandler(IJobRepository jobs)
    {
        _jobs = jobs;
    }

    public async Task<JobEditDto> Handle(GetJobForEditQuery request, CancellationToken cancellationToken)
    {
        var job = await _jobs.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Job), request.Id);

        return new JobEditDto
        {
            Id = job.Id,
            Title = job.Title,
            CompanyName = job.CompanyName,
            Description = job.Description,
            SignVideoUrl = job.SignVideoUrl,
            Location = job.Location,
            IsRemote = job.IsRemote,
            IsAccessibilityFriendly = job.IsAccessibilityFriendly,
            ApplyUrl = job.ApplyUrl,
            ContactEmail = job.ContactEmail,
            IsActive = job.IsActive
        };
    }
}
