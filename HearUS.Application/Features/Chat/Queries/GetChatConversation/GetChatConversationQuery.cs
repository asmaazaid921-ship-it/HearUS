using HearUS.Application.Features.Chat.Dtos;
using MediatR;

namespace HearUS.Application.Features.Chat.Queries.GetChatConversation;

public sealed record GetChatConversationQuery(int Id) : IRequest<ChatConversationDto>;
