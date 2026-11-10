using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Chat.Commands.MarkChatRead;

public sealed record MarkChatReadCommand(int ConversationId) : IRequest<int>;

public sealed class MarkChatReadCommandHandler : IRequestHandler<MarkChatReadCommand, int>
{
    private readonly IChatRepository _chats;
    private readonly ICurrentUserService _currentUser;

    public MarkChatReadCommandHandler(IChatRepository chats, ICurrentUserService currentUser)
    {
        _chats = chats;
        _currentUser = currentUser;
    }

    public async Task<int> Handle(MarkChatReadCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("يجب تسجيل الدخول للمشاركة في الدردشة.");

        var conversation = await _chats.GetConversationAsync(request.ConversationId)
            ?? throw new NotFoundException(nameof(ChatConversation), request.ConversationId);

        if (conversation.ParticipantOneId != userId && conversation.ParticipantTwoId != userId)
        {
            throw new ForbiddenAccessException("لا تملك صلاحية الوصول لهذه المحادثة.");
        }

        return await _chats.MarkReadAsync(request.ConversationId, userId);
    }
}
