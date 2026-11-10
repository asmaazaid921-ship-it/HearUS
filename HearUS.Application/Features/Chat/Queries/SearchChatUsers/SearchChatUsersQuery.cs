using HearUS.Application.Features.Chat.Dtos;
using MediatR;

namespace HearUS.Application.Features.Chat.Queries.SearchChatUsers;

public sealed record SearchChatUsersQuery(string? Q) : IRequest<ChatUserSearchResult>;
