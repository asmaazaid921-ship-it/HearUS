using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Signs.Common;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Signs.Commands.UpdateSignTerm;

public sealed record UpdateSignTermCommand(
    int Id,
    string? Term,
    string? Description,
    SignCategory Category,
    string? VideoUrl) : IRequest<Unit>, ISignDraft;

public sealed class UpdateSignTermCommandHandler : IRequestHandler<UpdateSignTermCommand, Unit>
{
    private readonly ISignTermRepository _signTerms;
    private readonly IApplicationDbContext _dbContext;

    public UpdateSignTermCommandHandler(ISignTermRepository signTerms, IApplicationDbContext dbContext)
    {
        _signTerms = signTerms;
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(UpdateSignTermCommand request, CancellationToken cancellationToken)
    {
        var term = await _signTerms.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(SignTerm), request.Id);

        term.Term = request.Term!.Trim();
        term.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
        term.Category = request.Category;
        term.VideoUrl = request.VideoUrl!.Trim();
        term.UpdatedAt = DateTime.UtcNow;

        _signTerms.Update(term);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public sealed class UpdateSignTermCommandValidator : SignDraftValidator<UpdateSignTermCommand>
{
}
