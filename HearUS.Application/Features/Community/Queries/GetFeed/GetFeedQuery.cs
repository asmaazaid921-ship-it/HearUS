using HearUS.Application.Features.Community.Dtos;
using MediatR;

namespace HearUS.Application.Features.Community.Queries.GetFeed;

public sealed record GetFeedQuery(int Page = 1) : IRequest<FeedResultDto>;
