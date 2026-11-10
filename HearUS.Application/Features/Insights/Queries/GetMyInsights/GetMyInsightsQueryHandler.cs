using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Insights.Dtos;
using MediatR;

namespace HearUS.Application.Features.Insights.Queries.GetMyInsights;

public sealed class GetMyInsightsQueryHandler : IRequestHandler<GetMyInsightsQuery, MyInsightsResult>
{
    private const int PageSize = 10;

    private readonly IInsightRepository _insights;
    private readonly ICurrentUserService _currentUser;

    public GetMyInsightsQueryHandler(
        IInsightRepository insights,
        ICurrentUserService currentUser)
    {
        _insights = insights;
        _currentUser = currentUser;
    }

    public async Task<MyInsightsResult> Handle(
        GetMyInsightsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("يجب تسجيل الدخول لعرض مقالاتك.");

        var page = Math.Max(1, request.Page);
        var (items, totalCount) = await _insights.GetForAuthorAsync(userId, page, PageSize);

        return new MyInsightsResult
        {
            TotalCount = totalCount,
            Page = page,
            PageSize = PageSize,
            Items = items.Select(insight => new InsightSummaryDto
            {
                Id = insight.Id,
                Title = insight.Title,
                Summary = insight.Summary,
                Category = insight.Category,
                SignVideoUrl = insight.SignVideoUrl,
                AuthorName = insight.Author.FullName,
                IsPublished = insight.IsPublished,
                CreatedAt = insight.CreatedAt,
                PublishedAt = insight.PublishedAt
            }).ToList()
        };
    }
}
