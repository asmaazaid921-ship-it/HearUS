using FluentValidation;
using HearUS.Application.Common.Identity;
using HearUS.Application.Common.Interfaces;
using MediatR;

namespace HearUS.Application.Features.Accounts.Commands.Login;

public sealed record LoginCommand(string? Email, string? Password, bool RememberMe) : IRequest<SignInAttemptResult>;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, SignInAttemptResult>
{
    private readonly IIdentityService _identity;

    public LoginCommandHandler(IIdentityService identity)
    {
        _identity = identity;
    }

    public Task<SignInAttemptResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return _identity.PasswordSignInAsync(request.Email!.Trim(), request.Password!, request.RememberMe);
    }
}

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("البريد الإلكتروني مطلوب");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("صيغة البريد الإلكتروني غير صحيحة")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("كلمة المرور مطلوبة");
    }
}
