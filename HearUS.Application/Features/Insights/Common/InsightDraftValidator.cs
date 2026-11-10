using FluentValidation;
using HearUS.Application.Common.Video;

namespace HearUS.Application.Features.Insights.Common;

public interface IInsightDraft
{
    string? Title { get; }

    string? Summary { get; }

    string? Content { get; }

    string? SignVideoUrl { get; }
}

public abstract class InsightDraftValidator<T> : AbstractValidator<T>
    where T : IInsightDraft
{
    protected InsightDraftValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("العنوان مطلوب");

        RuleFor(x => x.Title)
            .Length(5, 150)
            .WithMessage("العنوان يجب أن يكون بين 5 و150 حرفاً")
            .When(x => !string.IsNullOrWhiteSpace(x.Title));

        RuleFor(x => x.Summary)
            .MaximumLength(500)
            .WithMessage("الملخص يجب ألا يتجاوز 500 حرف")
            .When(x => !string.IsNullOrWhiteSpace(x.Summary));

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("نص المقال مطلوب");

        RuleFor(x => x.SignVideoUrl)
            .MaximumLength(500)
            .WithMessage("رابط الفيديو يجب ألا يتجاوز 500 حرف")
            .When(x => !string.IsNullOrWhiteSpace(x.SignVideoUrl));

        RuleFor(x => x.SignVideoUrl)
            .Must(VideoEmbed.IsSupported)
            .WithMessage("أدخل رابط YouTube أو Vimeo أو فيديو مباشر (mp4/webm/ogg) يبدأ بـ https.")
            .When(x => !string.IsNullOrWhiteSpace(x.SignVideoUrl));
    }
}
