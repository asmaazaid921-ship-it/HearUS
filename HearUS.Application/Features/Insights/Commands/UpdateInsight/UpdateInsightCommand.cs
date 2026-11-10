using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Insights.Common;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Insights.Commands.UpdateInsight;

public sealed record UpdateInsightCommand(
    int Id,
    string? Title,
    string? Summary,
    string? Content,
    string? SignVideoUrl,
    InsightCategory Category,
    bool IsPublished) : IRequest<Unit>, IInsightDraft;

public sealed class UpdateInsightCommandHandler : IRequestHandler<UpdateInsightCommand, Unit>
{
    private readonly IInsightRepository _insights;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUser;

    public UpdateInsightCommandHandler(
        IInsightRepository insights,
        IApplicationDbContext dbContext,
        ICurrentUserService currentUser)
    {
        _insights = insights;
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(UpdateInsightCommand request, CancellationToken cancellationToken)
    {
        var insight = await _insights.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Insight), request.Id);

        InsightAccess.EnsureCanManage(insight, _currentUser);

        insight.Title = request.Title!.Trim();
        insight.Summary = string.IsNullOrWhiteSpace(request.Summary) ? null : request.Summary.Trim();
        insight.Content = request.Content!.Trim();
        insight.SignVideoUrl = string.IsNullOrWhiteSpace(request.SignVideoUrl) ? null : request.SignVideoUrl.Trim();
        insight.Category = request.Category;

        if (request.IsPublished && !insight.IsPublished)
        {
            insight.PublishedAt = DateTime.UtcNow;
        }
        else if (!request.IsPublished)
        {
            insight.PublishedAt = null;
        }

        insight.IsPublished = request.IsPublished;

        _insights.Update(insight);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public sealed class UpdateInsightCommandValidator : InsightDraftValidator<UpdateInsightCommand>
{
}
