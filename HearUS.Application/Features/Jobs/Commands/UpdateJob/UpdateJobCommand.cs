using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Jobs.Common;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Jobs.Commands.UpdateJob;

public sealed record UpdateJobCommand(
    int Id,
    string? Title,
    string? CompanyName,
    string? Description,
    string? SignVideoUrl,
    string? Location,
    bool IsRemote,
    bool IsAccessibilityFriendly,
    string? ApplyUrl,
    string? ContactEmail,
    bool IsActive) : IRequest<Unit>, IJobDraft;

public sealed class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand, Unit>
{
    private readonly IJobRepository _jobs;
    private readonly IApplicationDbContext _dbContext;

    public UpdateJobCommandHandler(IJobRepository jobs, IApplicationDbContext dbContext)
    {
        _jobs = jobs;
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
    {
        var job = await _jobs.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Job), request.Id);

        JobDraftMapper.Apply(request, job);
        job.IsRemote = request.IsRemote;
        job.IsAccessibilityFriendly = request.IsAccessibilityFriendly;

        _jobs.Update(job);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public sealed class UpdateJobCommandValidator : JobDraftValidator<UpdateJobCommand>
{
}
