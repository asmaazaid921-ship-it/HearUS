using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Signs.Common;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Signs.Commands.CreateSignTerm;

public sealed record CreateSignTermCommand(
    string? Term,
    string? Description,
    SignCategory Category,
    string? VideoUrl) : IRequest<int>, ISignDraft;

public sealed class CreateSignTermCommandHandler : IRequestHandler<CreateSignTermCommand, int>
{
    private readonly ISignTermRepository _signTerms;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUser;

    public CreateSignTermCommandHandler(
        ISignTermRepository signTerms,
        IApplicationDbContext dbContext,
        ICurrentUserService currentUser)
    {
        _signTerms = signTerms;
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<int> Handle(CreateSignTermCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("يجب تسجيل الدخول لإضافة إشارة.");

        var term = new SignTerm
        {
            Term = request.Term!.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
            Category = request.Category,
            VideoUrl = request.VideoUrl!.Trim(),
            CreatedById = userId
        };

        await _signTerms.AddAsync(term);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return term.Id;
    }
}

public sealed class CreateSignTermCommandValidator : SignDraftValidator<CreateSignTermCommand>
{
}
