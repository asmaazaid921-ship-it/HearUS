using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Signs.Dtos;
using HearUS.Domain.Constants;
using MediatR;

namespace HearUS.Application.Features.Signs.Queries.GetSignTerms;

public sealed class GetSignTermsQueryHandler : IRequestHandler<GetSignTermsQuery, SignListResult>
{
    private const int PageSize = 12;

    private readonly ISignTermRepository _signTerms;
    private readonly ICurrentUserService _currentUser;

    public GetSignTermsQueryHandler(ISignTermRepository signTerms, ICurrentUserService currentUser)
    {
        _signTerms = signTerms;
        _currentUser = currentUser;
    }

    public async Task<SignListResult> Handle(GetSignTermsQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var search = string.IsNullOrWhiteSpace(request.Search) ? null : request.Search.Trim();

        var (items, totalCount) = await _signTerms.GetAsync(page, PageSize, search, request.Category);

        return new SignListResult
        {
            TotalCount = totalCount,
            Page = page,
            PageSize = PageSize,
            CanManage = _currentUser.IsInRole(AppRoles.Expert) || _currentUser.IsInRole(AppRoles.Admin),
            Items = items.Select(term => new SignTermSummaryDto
            {
                Id = term.Id,
                Term = term.Term,
                Description = term.Description,
                Category = term.Category,
                CreatedAt = term.CreatedAt
            }).ToList()
        };
    }
}
