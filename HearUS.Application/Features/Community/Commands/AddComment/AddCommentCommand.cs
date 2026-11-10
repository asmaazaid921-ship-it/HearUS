using FluentValidation;
using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Community.Commands.AddComment;

public sealed record AddCommentCommand(int PostId, string? Content) : IRequest<Unit>;

public sealed class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Unit>
{
    private const int MaxLength = 1000;

    private readonly IPostRepository _posts;
    private readonly ICommentRepository _comments;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUser;

    public AddCommentCommandHandler(
        IPostRepository posts,
        ICommentRepository comments,
        IApplicationDbContext dbContext,
        ICurrentUserService currentUser)
    {
        _posts = posts;
        _comments = comments;
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("يجب تسجيل الدخول لإضافة تعليق.");

        _ = await _posts.GetByIdAsync(request.PostId)
            ?? throw new NotFoundException(nameof(Post), request.PostId);

        var content = request.Content!.Trim();
        if (content.Length > MaxLength)
        {
            content = content[..MaxLength];
        }

        await _comments.AddAsync(new Comment
        {
            PostId = request.PostId,
            AuthorId = userId,
            Content = content
        });

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public sealed class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        RuleFor(x => x.Content)
            .Must(content => !string.IsNullOrWhiteSpace(content))
            .WithMessage("لا يمكن إضافة تعليق فارغ.");
    }
}
