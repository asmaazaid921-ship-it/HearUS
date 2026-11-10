using HearUS.Application.Features.Chat.Dtos;

namespace HearUS.Application.Common.Interfaces;

public interface IChatNotifier
{
    Task NotifyMessageSentAsync(ChatMessageDto message, CancellationToken cancellationToken = default);
}
