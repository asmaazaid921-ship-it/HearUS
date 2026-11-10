using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Chat.Dtos;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Chat.Queries.GetChatConversation;

public sealed class GetChatConversationQueryHandler : IRequestHandler<GetChatConversationQuery, ChatConversationDto>
{
    private readonly IChatRepository _chats;
    private readonly ICurrentUserService _currentUser;

    public GetChatConversationQueryHandler(IChatRepository chats, ICurrentUserService currentUser)
    {
        _chats = chats;
        _currentUser = currentUser;
    }

    public async Task<ChatConversationDto> Handle(GetChatConversationQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("يجب تسجيل الدخول لعرض المحادثة.");

        var conversation = await _chats.GetConversationAsync(request.Id)
            ?? throw new NotFoundException(nameof(ChatConversation), request.Id);

        if (conversation.ParticipantOneId != userId && conversation.ParticipantTwoId != userId)
        {
            throw new ForbiddenAccessException("لا تملك صلاحية الوصول لهذه المحادثة.");
        }

        var other = conversation.ParticipantOneId == userId
            ? conversation.ParticipantTwo
            : conversation.ParticipantOne;

        var messages = await _chats.GetMessagesAsync(request.Id, 100);

        return new ChatConversationDto
        {
            ConversationId = conversation.Id,
            OtherName = other.FullName,
            OtherInitial = string.IsNullOrEmpty(other.FullName) ? "؟" : other.FullName[..1],
            CurrentUserId = userId,
            Messages = messages.Select(message => new ChatMessageViewDto
            {
                Id = message.Id,
                SenderId = message.SenderId,
                SenderName = message.Sender?.FullName ?? string.Empty,
                Content = message.Content,
                SentAt = message.SentAt
            }).ToList()
        };
    }
}
