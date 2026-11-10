using FluentValidation;
using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Features.Chat.Dtos;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Chat.Commands.SendChatMessage;

public sealed record SendChatMessageCommand(int ConversationId, string? Content) : IRequest<ChatMessageDto>;

public sealed class SendChatMessageCommandHandler : IRequestHandler<SendChatMessageCommand, ChatMessageDto>
{
    private const int MaxContentLength = 2000;

    private readonly IChatRepository _chats;
    private readonly IUserRepository _users;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUser;
    private readonly IChatNotifier _notifier;

    public SendChatMessageCommandHandler(
        IChatRepository chats,
        IUserRepository users,
        IApplicationDbContext dbContext,
        ICurrentUserService currentUser,
        IChatNotifier notifier)
    {
        _chats = chats;
        _users = users;
        _dbContext = dbContext;
        _currentUser = currentUser;
        _notifier = notifier;
    }

    public async Task<ChatMessageDto> Handle(SendChatMessageCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("يجب تسجيل الدخول للمشاركة في الدردشة.");

        var conversation = await _chats.GetConversationAsync(request.ConversationId)
            ?? throw new NotFoundException(nameof(ChatConversation), request.ConversationId);

        if (conversation.ParticipantOneId != userId && conversation.ParticipantTwoId != userId)
        {
            throw new ForbiddenAccessException("لا تملك صلاحية الوصول لهذه المحادثة.");
        }

        var sender = await _users.GetByIdAsync(userId)
            ?? throw new ForbiddenAccessException("يجب تسجيل الدخول للمشاركة في الدردشة.");

        var content = request.Content!.Trim();
        if (content.Length > MaxContentLength)
        {
            content = content[..MaxContentLength];
        }

        var message = new ChatMessage
        {
            ConversationId = conversation.Id,
            SenderId = userId,
            Content = content,
            SentAt = DateTime.UtcNow
        };

        await _chats.AddMessageAsync(message);
        conversation.LastMessageAt = message.SentAt;
        await _dbContext.SaveChangesAsync(cancellationToken);

        var dto = new ChatMessageDto
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            SenderId = message.SenderId,
            SenderName = sender.FullName,
            Content = message.Content,
            SentAt = message.SentAt
        };

        await _notifier.NotifyMessageSentAsync(dto, cancellationToken);

        return dto;
    }
}

public sealed class SendChatMessageCommandValidator : AbstractValidator<SendChatMessageCommand>
{
    public SendChatMessageCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("لا يمكن إرسال رسالة فارغة.");
    }
}
