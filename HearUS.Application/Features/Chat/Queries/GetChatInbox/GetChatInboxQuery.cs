using HearUS.Application.Features.Chat.Dtos;
using MediatR;

namespace HearUS.Application.Features.Chat.Queries.GetChatInbox;

public sealed record GetChatInboxQuery : IRequest<ChatInboxDto>;
