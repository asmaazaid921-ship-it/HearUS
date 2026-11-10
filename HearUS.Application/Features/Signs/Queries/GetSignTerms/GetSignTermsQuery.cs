using HearUS.Application.Features.Signs.Dtos;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Signs.Queries.GetSignTerms;

public sealed record GetSignTermsQuery(string? Search, SignCategory? Category, int Page = 1)
    : IRequest<SignListResult>;
