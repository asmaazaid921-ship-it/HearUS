using FluentValidation;
using HearUS.Application.Common.Video;

namespace HearUS.Application.Features.Jobs.Common;

public interface IJobDraft
{
    string? Title { get; }

    string? CompanyName { get; }

    string? Description { get; }

    string? SignVideoUrl { get; }

    string? Location { get; }

    string? ApplyUrl { get; }

    string? ContactEmail { get; }

    bool IsActive { get; }
}

public abstract class JobDraftValidator<T> : AbstractValidator<T>
    where T : IJobDraft
{
    protected JobDraftValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("عنوان الوظيفة مطلوب");

        RuleFor(x => x.Title)
            .Length(3, 150)
            .WithMessage("العنوان يجب أن يكون بين 3 و150 حرفاً")
            .When(x => !string.IsNullOrWhiteSpace(x.Title));

        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .WithMessage("اسم الشركة مطلوب");

        RuleFor(x => x.CompanyName)
            .MaximumLength(100)
            .WithMessage("اسم الشركة يجب ألا يتجاوز 100 حرف")
            .When(x => !string.IsNullOrWhiteSpace(x.CompanyName));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("وصف الوظيفة مطلوب");

        RuleFor(x => x.Description)
            .Length(20, 4000)
            .WithMessage("الوصف يجب أن يكون بين 20 و4000 حرف")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.SignVideoUrl)
            .MaximumLength(500)
            .WithMessage("رابط الفيديو يجب ألا يتجاوز 500 حرف")
            .When(x => !string.IsNullOrWhiteSpace(x.SignVideoUrl));

        RuleFor(x => x.SignVideoUrl)
            .Must(VideoEmbed.IsSupported)
            .WithMessage("أدخل رابط YouTube أو Vimeo أو فيديو مباشر (mp4/webm/ogg) يبدأ بـ https.")
            .When(x => !string.IsNullOrWhiteSpace(x.SignVideoUrl));

        RuleFor(x => x.Location)
            .MaximumLength(100)
            .WithMessage("الموقع يجب ألا يتجاوز 100 حرف")
            .When(x => !string.IsNullOrWhiteSpace(x.Location));

        RuleFor(x => x.ApplyUrl)
            .MaximumLength(300)
            .WithMessage("رابط التقديم يجب ألا يتجاوز 300 حرف")
            .When(x => !string.IsNullOrWhiteSpace(x.ApplyUrl));

        RuleFor(x => x.ApplyUrl)
            .Must(BeWebUrl)
            .WithMessage("رابط التقديم غير صالح")
            .When(x => !string.IsNullOrWhiteSpace(x.ApplyUrl));

        RuleFor(x => x.ContactEmail)
            .MaximumLength(150)
            .WithMessage("البريد الإلكتروني يجب ألا يتجاوز 150 حرف")
            .When(x => !string.IsNullOrWhiteSpace(x.ContactEmail));

        RuleFor(x => x.ContactEmail)
            .EmailAddress()
            .WithMessage("البريد الإلكتروني غير صالح")
            .When(x => !string.IsNullOrWhiteSpace(x.ContactEmail));
    }

    private static bool BeWebUrl(string? value) =>
        Uri.TryCreate(value, UriKind.Absolute, out var uri)
        && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeFtp);
}
