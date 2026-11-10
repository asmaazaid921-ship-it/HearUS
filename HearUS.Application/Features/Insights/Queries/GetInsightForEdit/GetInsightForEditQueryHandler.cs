using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Insights.Common;
using HearUS.Application.Features.Insights.Dtos;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Insights.Queries.GetInsightForEdit;

public sealed class GetInsightForEditQueryHandler
    : IRequestHandler<GetInsightForEditQuery, InsightEditDto>
{
    private readonly IInsightRepository _insights;
    private readonly ICurrentUserService _currentUser;

    public GetInsightForEditQueryHandler(
        IInsightRepository insights,
        ICurrentUserService currentUser)
    {
        _insights = insights;
        _currentUser = currentUser;
    }

    public async Task<InsightEditDto> Handle(
        GetInsightForEditQuery request,
        CancellationToken cancellationToken)
    {
        var insight = await _insights.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Insight), request.Id);

        InsightAccess.EnsureCanManage(insight, _currentUser);

        return new InsightEditDto
        {
            Id = insight.Id,
            Title = insight.Title,
            Summary = insight.Summary,
            Content = insight.Content,
            SignVideoUrl = insight.SignVideoUrl,
            Category = insight.Category,
            IsPublished = insight.IsPublished
        };
    }
}
