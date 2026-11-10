using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Jobs.Commands.ToggleJobActive;

public sealed record ToggleJobActiveCommand(int Id) : IRequest<bool>;

public sealed class ToggleJobActiveCommandHandler : IRequestHandler<ToggleJobActiveCommand, bool>
{
    private readonly IJobRepository _jobs;
    private readonly IApplicationDbContext _dbContext;

    public ToggleJobActiveCommandHandler(IJobRepository jobs, IApplicationDbContext dbContext)
    {
        _jobs = jobs;
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(ToggleJobActiveCommand request, CancellationToken cancellationToken)
    {
        var job = await _jobs.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Job), request.Id);

        job.IsActive = !job.IsActive;

        _jobs.Update(job);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return job.IsActive;
    }
}
