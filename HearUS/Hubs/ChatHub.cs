using FluentValidation;
using HearUS.Application.Common.Exceptions;
using HearUS.Application.Features.Chat.Commands.MarkChatRead;
using HearUS.Application.Features.Chat.Commands.SendChatMessage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HearUS.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ISender _mediator;

        public ChatHub(ISender mediator)
        {
            _mediator = mediator;
        }

        public static string GroupName(int conversationId) => $"chat-{conversationId}";

        public async Task JoinConversation(int conversationId)
        {
            try
            {
                await _mediator.Send(new MarkChatReadCommand(conversationId));
            }
            catch (ForbiddenAccessException)
            {
                throw new HubException("لا تملك صلاحية الوصول لهذه المحادثة.");
            }
            catch (NotFoundException)
            {
                throw new HubException("المحادثة غير موجودة.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, GroupName(conversationId));
        }

        public async Task SendMessage(int conversationId, string? content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return;
            }

            try
            {
                await _mediator.Send(new SendChatMessageCommand(conversationId, content));
            }
            catch (ValidationException)
            {
                return;
            }
            catch (ForbiddenAccessException)
            {
                throw new HubException("لا تملك صلاحية الوصول لهذه المحادثة.");
            }
            catch (NotFoundException)
            {
                throw new HubException("المحادثة غير موجودة.");
            }
        }
    }
}
