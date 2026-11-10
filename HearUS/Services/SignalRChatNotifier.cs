using HearUS.Application.Common.Interfaces;
using HearUS.Application.Features.Chat.Dtos;
using HearUS.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace HearUS.Web.Services;

public sealed class SignalRChatNotifier : IChatNotifier
{
    private readonly IHubContext<ChatHub> _hubContext;

    public SignalRChatNotifier(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task NotifyMessageSentAsync(ChatMessageDto message, CancellationToken cancellationToken = default)
    {
        return _hubContext.Clients
            .Group(ChatHub.GroupName(message.ConversationId))
            .SendAsync("ReceiveMessage", message, cancellationToken);
    }
}
