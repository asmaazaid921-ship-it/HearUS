using HearUS.ViewModels;

namespace HearUS.Areas.Community.ViewModels
{
    public class FeedViewModel
    {
        public IReadOnlyList<PostCardViewModel> Posts { get; set; } = Array.Empty<PostCardViewModel>();

        public PagerViewModel Pager { get; set; } = new();

        public bool IsComposerVisible { get; set; }
    }

    public class PostCardViewModel
    {
        public int Id { get; set; }

        public string AuthorName { get; set; } = string.Empty;

        public string AuthorInitial { get; set; } = string.Empty;

        public bool IsExpertAuthor { get; set; }

        public string Content { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public string? SignVideoUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public int LikesCount { get; set; }

        public bool IsLikedByCurrentUser { get; set; }

        public bool CanDelete { get; set; }

        public IReadOnlyList<CommentViewModel> Comments { get; set; } = Array.Empty<CommentViewModel>();
    }

    public class CommentViewModel
    {
        public int Id { get; set; }

        public string AuthorName { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
