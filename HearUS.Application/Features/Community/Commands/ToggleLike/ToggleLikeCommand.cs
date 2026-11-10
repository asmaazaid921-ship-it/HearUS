using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Community.Dtos;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Community.Commands.ToggleLike;

public sealed record ToggleLikeCommand(int PostId) : IRequest<ToggleLikeResultDto>;

public sealed class ToggleLikeCommandHandler : IRequestHandler<ToggleLikeCommand, ToggleLikeResultDto>
{
    private readonly IPostRepository _posts;
    private readonly IPostLikeRepository _likes;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUser;

    public ToggleLikeCommandHandler(
        IPostRepository posts,
        IPostLikeRepository likes,
        IApplicationDbContext dbContext,
        ICurrentUserService currentUser)
    {
        _posts = posts;
        _likes = likes;
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<ToggleLikeResultDto> Handle(ToggleLikeCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("يجب تسجيل الدخول للإعجاب بمنشور.");

        _ = await _posts.GetByIdAsync(request.PostId)
            ?? throw new NotFoundException(nameof(Post), request.PostId);

        var existingLike = await _likes.FindAsync(request.PostId, userId);
        var liked = existingLike is null;

        if (existingLike is null)
        {
            await _likes.AddAsync(new PostLike
            {
                PostId = request.PostId,
                UserId = userId
            });
        }
        else
        {
            _likes.Remove(existingLike);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        var count = await _likes.CountForPostAsync(request.PostId);

        return new ToggleLikeResultDto(liked, count);
    }
}
