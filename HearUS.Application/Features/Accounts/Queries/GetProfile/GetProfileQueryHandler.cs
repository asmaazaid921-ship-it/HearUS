using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Accounts.Dtos;
using MediatR;

namespace HearUS.Application.Features.Accounts.Queries.GetProfile;

public sealed class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserProfileDto>
{
    private readonly IIdentityService _identity;
    private readonly IPostRepository _posts;
    private readonly IInsightRepository _insights;
    private readonly ICurrentUserService _currentUser;

    public GetProfileQueryHandler(
        IIdentityService identity,
        IPostRepository posts,
        IInsightRepository insights,
        ICurrentUserService currentUser)
    {
        _identity = identity;
        _posts = posts;
        _insights = insights;
        _currentUser = currentUser;
    }

    public async Task<UserProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("يجب تسجيل الدخول لعرض الملف الشخصي.");

        var user = await _identity.GetUserAsync(userId)
            ?? throw new ForbiddenAccessException("يجب تسجيل الدخول لعرض الملف الشخصي.");

        var roles = await _identity.GetRolesAsync(userId);
        var insights = await _insights.GetForAuthorAsync(userId, 1, 1);
        var postsCount = await _posts.CountForUserAsync(userId);

        return new UserProfileDto
        {
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            City = user.City,
            Bio = user.Bio,
            CreatedAt = user.CreatedAt,
            Roles = roles,
            PostsCount = postsCount,
            InsightsCount = insights.TotalCount
        };
    }
}
