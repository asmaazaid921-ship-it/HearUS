using FluentValidation;
using HearUS.Application.Features.Chat.Commands.SendChatMessage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HearUS.Areas.Chat.Controllers
{
    [Area("Chat")]
    [Route("Chat/Messages")]
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly ISender _mediator;

        public MessagesController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Send")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(int conversationId, string? content, CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(new SendChatMessageCommand(conversationId, content), cancellationToken);
            }
            catch (ValidationException exception)
            {
                TempData["Error"] = exception.Errors.First().ErrorMessage;
            }

            return RedirectToAction("Conversation", "Home", new { area = "Chat", id = conversationId });
        }
    }
}
