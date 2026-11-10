using HearUS.Application.Features.Insights.Dtos;
using MediatR;

namespace HearUS.Application.Features.Insights.Queries.GetInsightForEdit;

public sealed record GetInsightForEditQuery(int Id) : IRequest<InsightEditDto>;
