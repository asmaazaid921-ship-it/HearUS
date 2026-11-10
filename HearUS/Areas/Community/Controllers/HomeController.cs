using HearUS.Areas.Community.ViewModels;
using HearUS.Application.Features.Community.Queries.GetFeed;
using HearUS.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HearUS.Areas.Community.Controllers
{
    [Area("Community")]
    public class HomeController : Controller
    {
        private readonly ISender _mediator;

        public HomeController(ISender mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(int page = 1, CancellationToken cancellationToken = default)
        {
            var feed = await _mediator.Send(new GetFeedQuery(page), cancellationToken);

            return View(new FeedViewModel
            {
                IsComposerVisible = feed.IsComposerVisible,
                Posts = feed.Items.Select(item => new PostCardViewModel
                {
                    Id = item.Id,
                    AuthorName = item.AuthorName,
                    AuthorInitial = string.IsNullOrEmpty(item.AuthorName) ? "؟" : item.AuthorName[..1],
                    IsExpertAuthor = item.IsExpertAuthor,
                    Content = item.Content,
                    ImageUrl = item.ImageUrl,
                    SignVideoUrl = item.SignVideoUrl,
                    CreatedAt = item.CreatedAt,
                    LikesCount = item.LikesCount,
                    IsLikedByCurrentUser = item.IsLikedByCurrentUser,
                    CanDelete = item.CanDelete,
                    Comments = item.Comments.Select(comment => new CommentViewModel
                    {
                        Id = comment.Id,
                        AuthorName = comment.AuthorName,
                        Content = comment.Content,
                        CreatedAt = comment.CreatedAt
                    }).ToList()
                }).ToList(),
                Pager = new PagerViewModel
                {
                    Page = feed.Page,
                    TotalPages = (int)Math.Ceiling(feed.TotalCount / (double)feed.PageSize),
                    Area = "Community",
                    Controller = "Home",
                    Action = "Index"
                }
            });
        }
    }
}
