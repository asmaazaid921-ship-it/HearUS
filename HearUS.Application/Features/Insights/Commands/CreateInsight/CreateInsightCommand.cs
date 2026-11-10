using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Insights.Common;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Insights.Commands.CreateInsight;

public sealed record CreateInsightCommand(
    string? Title,
    string? Summary,
    string? Content,
    string? SignVideoUrl,
    InsightCategory Category,
    bool IsPublished) : IRequest<int>, IInsightDraft;

public sealed class CreateInsightCommandHandler : IRequestHandler<CreateInsightCommand, int>
{
    private readonly IInsightRepository _insights;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUser;

    public CreateInsightCommandHandler(
        IInsightRepository insights,
        IApplicationDbContext dbContext,
        ICurrentUserService currentUser)
    {
        _insights = insights;
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<int> Handle(CreateInsightCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("يجب تسجيل الدخول لكتابة مقال.");

        var insight = new Insight
        {
            AuthorId = userId,
            Title = request.Title!.Trim(),
            Summary = string.IsNullOrWhiteSpace(request.Summary) ? null : request.Summary.Trim(),
            Content = request.Content!.Trim(),
            SignVideoUrl = string.IsNullOrWhiteSpace(request.SignVideoUrl) ? null : request.SignVideoUrl.Trim(),
            Category = request.Category,
            IsPublished = request.IsPublished,
            PublishedAt = request.IsPublished ? DateTime.UtcNow : null
        };

        await _insights.AddAsync(insight);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return insight.Id;
    }
}

public sealed class CreateInsightCommandValidator : InsightDraftValidator<CreateInsightCommand>
{
}
