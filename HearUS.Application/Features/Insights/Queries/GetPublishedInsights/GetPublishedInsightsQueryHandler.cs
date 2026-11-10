using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Insights.Dtos;
using HearUS.Domain.Constants;
using MediatR;

namespace HearUS.Application.Features.Insights.Queries.GetPublishedInsights;

public sealed class GetPublishedInsightsQueryHandler
    : IRequestHandler<GetPublishedInsightsQuery, InsightListResult>
{
    private const int PageSize = 9;

    private readonly IInsightRepository _insights;
    private readonly ICurrentUserService _currentUser;

    public GetPublishedInsightsQueryHandler(
        IInsightRepository insights,
        ICurrentUserService currentUser)
    {
        _insights = insights;
        _currentUser = currentUser;
    }

    public async Task<InsightListResult> Handle(
        GetPublishedInsightsQuery request,
        CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var (items, totalCount) = await _insights.GetPublishedAsync(page, PageSize, request.Category);

        return new InsightListResult
        {
            TotalCount = totalCount,
            Page = page,
            PageSize = PageSize,
            CanWrite = _currentUser.IsInRole(AppRoles.Expert) || _currentUser.IsInRole(AppRoles.Admin),
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
