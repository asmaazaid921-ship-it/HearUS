using HearUS.Application.Features.Signs.Dtos;
using MediatR;

namespace HearUS.Application.Features.Signs.Queries.GetSignTermDetails;

public sealed record GetSignTermDetailsQuery(int Id) : IRequest<SignTermDetailsDto>;
