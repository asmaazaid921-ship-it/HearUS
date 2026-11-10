using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Jobs.Dtos;
using HearUS.Domain.Constants;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Jobs.Queries.GetJobDetails;

public sealed class GetJobDetailsQueryHandler : IRequestHandler<GetJobDetailsQuery, JobDetailsDto>
{
    private readonly IJobRepository _jobs;
    private readonly ICurrentUserService _currentUser;

    public GetJobDetailsQueryHandler(IJobRepository jobs, ICurrentUserService currentUser)
    {
        _jobs = jobs;
        _currentUser = currentUser;
    }

    public async Task<JobDetailsDto> Handle(GetJobDetailsQuery request, CancellationToken cancellationToken)
    {
        var job = await _jobs.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Job), request.Id);

        if (!job.IsActive && !_currentUser.IsInRole(AppRoles.Admin))
        {
            throw new NotFoundException(nameof(Job), request.Id);
        }

        return new JobDetailsDto
        {
            Id = job.Id,
            Title = job.Title,
            CompanyName = job.CompanyName,
            Description = job.Description,
            Location = job.Location,
            IsRemote = job.IsRemote,
            IsAccessibilityFriendly = job.IsAccessibilityFriendly,
            SignVideoUrl = job.SignVideoUrl,
            ApplyUrl = job.ApplyUrl,
            ContactEmail = job.ContactEmail,
            PostedAt = job.PostedAt,
            IsActive = job.IsActive
        };
    }
}
