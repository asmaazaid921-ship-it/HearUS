using HearUS.Application.Features.Accounts.Dtos;
using MediatR;

namespace HearUS.Application.Features.Accounts.Queries.GetProfile;

public sealed record GetProfileQuery : IRequest<UserProfileDto>;
