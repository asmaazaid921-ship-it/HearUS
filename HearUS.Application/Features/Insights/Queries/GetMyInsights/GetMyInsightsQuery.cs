using HearUS.Application.Features.Insights.Dtos;
using MediatR;

namespace HearUS.Application.Features.Insights.Queries.GetMyInsights;

public sealed record GetMyInsightsQuery(int Page = 1) : IRequest<MyInsightsResult>;
