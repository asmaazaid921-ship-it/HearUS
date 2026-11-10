using FluentValidation;
using FluentValidation.Results;
using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Chat.Commands.StartConversation;

public sealed record StartConversationCommand(string UserId) : IRequest<int>;

public sealed class StartConversationCommandHandler : IRequestHandler<StartConversationCommand, int>
{
    private readonly IChatRepository _chats;
    private readonly IUserRepository _users;
    private readonly ICurrentUserService _currentUser;

    public StartConversationCommandHandler(
        IChatRepository chats,
        IUserRepository users,
        ICurrentUserService currentUser)
    {
        _chats = chats;
        _users = users;
        _currentUser = currentUser;
    }

    public async Task<int> Handle(StartConversationCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("يجب تسجيل الدخول لبدء محادثة.");

        if (string.Equals(request.UserId, currentUserId, StringComparison.Ordinal))
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(request.UserId), "لا يمكنك بدء محادثة مع نفسك.")
            });
        }

        var other = await _users.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException(nameof(ApplicationUser), request.UserId);

        var (participantOneId, participantTwoId) =
            string.CompareOrdinal(currentUserId, other.Id) < 0
                ? (currentUserId, other.Id)
                : (other.Id, currentUserId);

        var conversation = await _chats.GetOrCreateBetweenAsync(participantOneId, participantTwoId);

        return conversation.Id;
    }
}
