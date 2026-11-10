using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Domain.Constants;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Community.Commands.DeletePost;

public sealed record DeletePostCommand(int PostId) : IRequest<Unit>;

public sealed class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Unit>
{
    private readonly IPostRepository _posts;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUser;

    public DeletePostCommandHandler(
        IPostRepository posts,
        IApplicationDbContext dbContext,
        ICurrentUserService currentUser)
    {
        _posts = posts;
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("يجب تسجيل الدخول لحذف منشور.");

        var post = await _posts.GetByIdAsync(request.PostId)
            ?? throw new NotFoundException(nameof(Post), request.PostId);

        if (post.AuthorId != userId && !_currentUser.IsInRole(AppRoles.Admin))
        {
            throw new ForbiddenAccessException();
        }

        post.IsDeleted = true;
        _posts.Update(post);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
