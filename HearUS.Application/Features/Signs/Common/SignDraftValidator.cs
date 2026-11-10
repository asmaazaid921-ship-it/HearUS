using FluentValidation;
using HearUS.Application.Common.Video;
using HearUS.Domain.Entities;

namespace HearUS.Application.Features.Signs.Common;

public interface ISignDraft
{
    string? Term { get; }

    string? Description { get; }

    SignCategory Category { get; }

    string? VideoUrl { get; }
}

public abstract class SignDraftValidator<T> : AbstractValidator<T>
    where T : ISignDraft
{
    protected SignDraftValidator()
    {
        RuleFor(x => x.Term)
            .NotEmpty()
            .WithMessage("الكلمة أو العبارة مطلوبة");

        RuleFor(x => x.Term)
            .Length(2, 100)
            .WithMessage("الكلمة يجب أن تكون بين 2 و100 حرف")
            .When(x => !string.IsNullOrWhiteSpace(x.Term));

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("الوصف يجب ألا يتجاوز 500 حرف")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Category)
            .IsInEnum()
            .WithMessage("التصنيف غير صالح");

        RuleFor(x => x.VideoUrl)
            .NotEmpty()
            .WithMessage("رابط الفيديو مطلوب");

        RuleFor(x => x.VideoUrl)
            .MaximumLength(500)
            .WithMessage("رابط الفيديو يجب ألا يتجاوز 500 حرف")
            .When(x => !string.IsNullOrWhiteSpace(x.VideoUrl));

        RuleFor(x => x.VideoUrl)
            .Must(VideoEmbed.IsSupported)
            .WithMessage("أدخل رابط YouTube أو Vimeo أو فيديو مباشر (mp4/webm/ogg) يبدأ بـ https.")
            .When(x => !string.IsNullOrWhiteSpace(x.VideoUrl));
    }
}
