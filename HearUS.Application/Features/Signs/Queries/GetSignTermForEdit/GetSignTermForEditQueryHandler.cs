using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Signs.Dtos;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Signs.Queries.GetSignTermForEdit;

public sealed class GetSignTermForEditQueryHandler : IRequestHandler<GetSignTermForEditQuery, SignTermEditDto>
{
    private readonly ISignTermRepository _signTerms;

    public GetSignTermForEditQueryHandler(ISignTermRepository signTerms)
    {
        _signTerms = signTerms;
    }

    public async Task<SignTermEditDto> Handle(GetSignTermForEditQuery request, CancellationToken cancellationToken)
    {
        var term = await _signTerms.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(SignTerm), request.Id);

        return new SignTermEditDto
        {
            Id = term.Id,
            Term = term.Term,
            Description = term.Description,
            Category = term.Category,
            VideoUrl = term.VideoUrl
        };
    }
}
