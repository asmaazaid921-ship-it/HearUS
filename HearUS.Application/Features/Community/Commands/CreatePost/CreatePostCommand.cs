using FluentValidation;
using HearUS.Application.Common.Exceptions;
using HearUS.Application.Common.Interfaces;
using HearUS.Application.Common.Interfaces.Repositories;
using HearUS.Application.Common.Video;
using HearUS.Domain.Entities;
using MediatR;

namespace HearUS.Application.Features.Community.Commands.CreatePost;

public sealed record CreatePostCommand(string? Content, string? ImageUrl, string? SignVideoUrl) : IRequest<int>;

public sealed class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, int>
{
    private readonly IPostRepository _posts;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUser;

    public CreatePostCommandHandler(
        IPostRepository posts,
        IApplicationDbContext dbContext,
        ICurrentUserService currentUser)
    {
        _posts = posts;
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<int> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("يجب تسجيل الدخول لنشر منشور.");

        var post = new Post
        {
            AuthorId = userId,
            Content = request.Content!.Trim(),
            ImageUrl = Normalize(request.ImageUrl),
            SignVideoUrl = Normalize(request.SignVideoUrl)
        };

        await _posts.AddAsync(post);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return post.Id;
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

public sealed class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Content)
            .Must(content => !string.IsNullOrWhiteSpace(content))
            .WithMessage("لا يمكن نشر منشور فارغ.");

        RuleFor(x => x.Content)
            .MaximumLength(2000)
            .WithMessage("المنشور أطول من الحد المسموح (2000 حرف).")
            .When(x => !string.IsNullOrWhiteSpace(x.Content));

        RuleFor(x => x.ImageUrl)
            .Must(BeSafeImageUrl)
            .WithMessage("رابط الصورة غير صالح. استخدم رابطاً يبدأ بـ http أو https.")
            .When(x => !string.IsNullOrWhiteSpace(x.ImageUrl));

        RuleFor(x => x.SignVideoUrl)
            .Must(VideoEmbed.IsSupported)
            .WithMessage("رابط فيديو الإشارة غير صالح. استخدم رابط YouTube أو Vimeo أو ملف mp4/webm/ogg.")
            .When(x => !string.IsNullOrWhiteSpace(x.SignVideoUrl));
    }

    private static bool BeSafeImageUrl(string? value) =>
        Uri.TryCreate(value, UriKind.Absolute, out var uri)
        && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
}
