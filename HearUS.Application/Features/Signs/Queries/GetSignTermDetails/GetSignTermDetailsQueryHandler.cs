using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Signs.Dtos;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Signs.Queries.GetSignTermDetails;

public sealed class GetSignTermDetailsQueryHandler : IRequestHandler<GetSignTermDetailsQuery, SignTermDetailsDto>
{
    private readonly ISignTermRepository _signTerms;

    public GetSignTermDetailsQueryHandler(ISignTermRepository signTerms)
    {
        _signTerms = signTerms;
    }

    public async Task<SignTermDetailsDto> Handle(GetSignTermDetailsQuery request, CancellationToken cancellationToken)
    {
        var term = await _signTerms.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(SignTerm), request.Id);

        return new SignTermDetailsDto
        {
            Id = term.Id,
            Term = term.Term,
            Description = term.Description,
            Category = term.Category,
            VideoUrl = term.VideoUrl,
            AuthorName = term.CreatedBy?.FullName ?? string.Empty,
            CreatedAt = term.CreatedAt,
            UpdatedAt = term.UpdatedAt
        };
    }
}
