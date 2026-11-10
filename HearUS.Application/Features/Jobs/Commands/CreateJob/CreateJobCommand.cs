using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Jobs.Common;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Jobs.Commands.CreateJob;

public sealed record CreateJobCommand(
    string? Title,
    string? CompanyName,
    string? Description,
    string? SignVideoUrl,
    string? Location,
    bool IsRemote,
    bool IsAccessibilityFriendly,
    string? ApplyUrl,
    string? ContactEmail,
    bool IsActive) : IRequest<int>, IJobDraft;

public sealed class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, int>
{
    private readonly IJobRepository _jobs;
    private readonly IApplicationDbContext _dbContext;

    public CreateJobCommandHandler(IJobRepository jobs, IApplicationDbContext dbContext)
    {
        _jobs = jobs;
        _dbContext = dbContext;
    }

    public async Task<int> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        var job = JobDraftMapper.Apply(request, new Job
        {
            IsRemote = request.IsRemote,
            IsAccessibilityFriendly = request.IsAccessibilityFriendly
        });

        await _jobs.AddAsync(job);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return job.Id;
    }
}

public sealed class CreateJobCommandValidator : JobDraftValidator<CreateJobCommand>
{
}
