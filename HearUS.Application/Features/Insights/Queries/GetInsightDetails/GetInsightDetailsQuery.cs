using HearUS.Application.Features.Insights.Dtos;
using MediatR;

namespace HearUS.Application.Features.Insights.Queries.GetInsightDetails;

public sealed record GetInsightDetailsQuery(int Id) : IRequest<InsightDetailsDto>;
