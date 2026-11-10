using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Community.Dtos;
using HearUS.Domain.Constants;
using MediatR;

namespace HearUS.Application.Features.Community.Queries.GetFeed;

public sealed class GetFeedQueryHandler : IRequestHandler<GetFeedQuery, FeedResultDto>
{
    private const int PageSize = 10;

    private readonly IPostRepository _posts;
    private readonly IUserRepository _users;
    private readonly ICurrentUserService _currentUser;

    public GetFeedQueryHandler(
        IPostRepository posts,
        IUserRepository users,
        ICurrentUserService currentUser)
    {
        _posts = posts;
        _users = users;
        _currentUser = currentUser;
    }

    public async Task<FeedResultDto> Handle(GetFeedQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var (items, totalCount) = await _posts.GetFeedAsync(page, PageSize);

        var userId = _currentUser.UserId;
        var isAdmin = _currentUser.IsInRole(AppRoles.Admin);
        var expertIds = await _users.GetUserIdsInRoleAsync(AppRoles.Expert);

        var likedIds = userId is null
            ? new HashSet<int>()
            : await _posts.GetLikedPostIdsAsync(userId, items.Select(p => p.Id));

        return new FeedResultDto
        {
            Page = page,
            PageSize = PageSize,
            TotalCount = totalCount,
            IsComposerVisible = userId is not null,
            Items = items.Select(post => new FeedItemDto
            {
                Id = post.Id,
                AuthorName = post.Author.FullName,
                IsExpertAuthor = expertIds.Contains(post.AuthorId),
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                SignVideoUrl = post.SignVideoUrl,
                CreatedAt = post.CreatedAt,
                LikesCount = post.Likes.Count,
                IsLikedByCurrentUser = likedIds.Contains(post.Id),
                CanDelete = isAdmin || post.AuthorId == userId,
                Comments = post.Comments
                    .OrderBy(c => c.CreatedAt)
                    .Select(c => new FeedCommentDto
                    {
                        Id = c.Id,
                        AuthorName = c.Author.FullName,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt
                    })
                    .ToList()
            }).ToList()
        };
    }
}
