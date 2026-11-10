using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Chat.Dtos;
using MediatR;

namespace HearUS.Application.Features.Chat.Queries.SearchChatUsers;

public sealed class SearchChatUsersQueryHandler : IRequestHandler<SearchChatUsersQuery, ChatUserSearchResult>
{
    private readonly IUserRepository _users;
    private readonly ICurrentUserService _currentUser;

    public SearchChatUsersQueryHandler(IUserRepository users, ICurrentUserService currentUser)
    {
        _users = users;
        _currentUser = currentUser;
    }

    public async Task<ChatUserSearchResult> Handle(SearchChatUsersQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("يجب تسجيل الدخول لبدء محادثة.");

        var users = await _users.SearchAsync(request.Q, userId);

        return new ChatUserSearchResult
        {
            Search = request.Q,
            Users = users.Select(user => new ChatUserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email
            }).ToList()
        };
    }
}
