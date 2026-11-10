using HearUS.Application.Features.Signs.Dtos;
using MediatR;

namespace HearUS.Application.Features.Signs.Queries.GetSignTermForEdit;

public sealed record GetSignTermForEditQuery(int Id) : IRequest<SignTermEditDto>;
