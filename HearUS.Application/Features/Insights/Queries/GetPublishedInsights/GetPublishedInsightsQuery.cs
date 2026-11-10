using HearUS.Application.Features.Insights.Dtos;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Insights.Queries.GetPublishedInsights;

public sealed record GetPublishedInsightsQuery(int Page = 1, InsightCategory? Category = null)
    : IRequest<InsightListResult>;
