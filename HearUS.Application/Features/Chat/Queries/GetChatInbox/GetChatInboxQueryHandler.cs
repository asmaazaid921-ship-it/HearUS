using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Chat.Dtos;
using MediatR;

namespace HearUS.Application.Features.Chat.Queries.GetChatInbox;

public sealed class GetChatInboxQueryHandler : IRequestHandler<GetChatInboxQuery, ChatInboxDto>
{
    private readonly IChatRepository _chats;
    private readonly ICurrentUserService _currentUser;

    public GetChatInboxQueryHandler(IChatRepository chats, ICurrentUserService currentUser)
    {
        _chats = chats;
        _currentUser = currentUser;
    }

    public async Task<ChatInboxDto> Handle(GetChatInboxQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("يجب تسجيل الدخول لعرض المحادثات.");

        var conversations = await _chats.GetConversationsForUserAsync(userId);

        return new ChatInboxDto
        {
            Items = conversations.Select(conversation =>
            {
                var other = conversation.ParticipantOneId == userId
                    ? conversation.ParticipantTwo
                    : conversation.ParticipantOne;

                var lastMessage = conversation.Messages
                    .OrderByDescending(m => m.SentAt)
                    .FirstOrDefault();

                return new ChatConversationSummaryDto
                {
                    Id = conversation.Id,
                    OtherName = other.FullName,
                    OtherInitial = string.IsNullOrEmpty(other.FullName) ? "؟" : other.FullName[..1],
                    LastMessage = lastMessage?.Content,
                    LastActivityAt = lastMessage?.SentAt ?? conversation.LastMessageAt
                };
            }).ToList()
        };
    }
}
