using FluentValidation;
using HearUS.Application.Features.Community.Commands.AddComment;
using HearUS.Application.Features.Community.Commands.CreatePost;
using HearUS.Application.Features.Community.Commands.DeletePost;
using HearUS.Application.Features.Community.Commands.ToggleLike;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HearUS.Areas.Community.Controllers
{
    [Area("Community")]
    [Authorize]
    public class PostsController : Controller
    {
        private readonly ISender _mediator;

        public PostsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            string? content,
            string? imageUrl,
            string? signVideoUrl,
            CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(new CreatePostCommand(content, imageUrl, signVideoUrl), cancellationToken);
                TempData["Success"] = "تم نشر منشورك في الحائط المشترك.";
            }
            catch (ValidationException exception)
            {
                TempData["Error"] = exception.Errors.First().ErrorMessage;
            }

            return RedirectToAction("Index", "Home", new { area = "Community" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeletePostCommand(id), cancellationToken);

            TempData["Success"] = "تم حذف المنشور.";
            return RedirectToAction("Index", "Home", new { area = "Community" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLike(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ToggleLikeCommand(id), cancellationToken);

            return Json(new { liked = result.Liked, count = result.Count });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int postId, string? content, CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(new AddCommentCommand(postId, content), cancellationToken);
            }
            catch (ValidationException exception)
            {
                TempData["Error"] = exception.Errors.First().ErrorMessage;
            }

            return Redirect($"{Url.Action("Index", "Home", new { area = "Community" })}#post-{postId}");
        }
    }
}
