using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Insights.Dtos;
using HearUS.Domain.Constants;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Insights.Queries.GetInsightDetails;

public sealed class GetInsightDetailsQueryHandler
    : IRequestHandler<GetInsightDetailsQuery, InsightDetailsDto>
{
    private readonly IInsightRepository _insights;
    private readonly ICurrentUserService _currentUser;

    public GetInsightDetailsQueryHandler(
        IInsightRepository insights,
        ICurrentUserService currentUser)
    {
        _insights = insights;
        _currentUser = currentUser;
    }

    public async Task<InsightDetailsDto> Handle(
        GetInsightDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var insight = await _insights.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Insight), request.Id);

        if (!insight.IsPublished &&
            insight.AuthorId != _currentUser.UserId &&
            !_currentUser.IsInRole(AppRoles.Admin))
        {
            throw new NotFoundException(nameof(Insight), request.Id);
        }

        return new InsightDetailsDto
        {
            Id = insight.Id,
            Title = insight.Title,
            Summary = insight.Summary,
            Content = insight.Content,
            Category = insight.Category,
            SignVideoUrl = insight.SignVideoUrl,
            AuthorName = insight.Author.FullName,
            IsPublished = insight.IsPublished,
            CreatedAt = insight.CreatedAt,
            PublishedAt = insight.PublishedAt
        };
    }
}
