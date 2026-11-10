using HearUS.Application.Common.Interfaces;
using MediatR;

namespace HearUS.Application.Features.Accounts.Commands.Logout;

public sealed record LogoutCommand : IRequest<Unit>;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IIdentityService _identity;

    public LogoutCommandHandler(IIdentityService identity)
    {
        _identity = identity;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _identity.SignOutAsync();

        return Unit.Value;
    }
}
