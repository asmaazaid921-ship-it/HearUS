using FluentValidation;
using HearUS.Application.Common.Identity;
using HearUS.Application.Common.Interfaces;
using MediatR;

namespace HearUS.Application.Features.Accounts.Commands.Register;

public sealed record RegisterCommand(
    string? FullName,
    string? Email,
    string? City,
    string? Password) : IRequest<RegisterUserResult>;

public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterUserResult>
{
    private readonly IIdentityService _identity;

    public RegisterCommandHandler(IIdentityService identity)
    {
        _identity = identity;
    }

    public Task<RegisterUserResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return _identity.RegisterAsync(
            request.FullName!.Trim(),
            request.Email!.Trim(),
            string.IsNullOrWhiteSpace(request.City) ? null : request.City.Trim(),
            request.Password!);
    }
}

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("الاسم الكامل مطلوب");

        RuleFor(x => x.FullName)
            .Length(3, 100)
            .WithMessage("الاسم يجب أن يكون بين 3 و100 حرف")
            .When(x => !string.IsNullOrWhiteSpace(x.FullName));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("البريد الإلكتروني مطلوب");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("صيغة البريد الإلكتروني غير صحيحة")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.City)
            .MaximumLength(50)
            .WithMessage("المدينة يجب ألا تتجاوز 50 حرفاً")
            .When(x => !string.IsNullOrWhiteSpace(x.City));

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("كلمة المرور مطلوبة");

        RuleFor(x => x.Password)
            .Length(8, 100)
            .WithMessage("كلمة المرور يجب أن تكون 8 أحرف على الأقل")
            .When(x => !string.IsNullOrWhiteSpace(x.Password));
    }
}
