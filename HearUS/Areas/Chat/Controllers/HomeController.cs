using FluentValidation;
using HearUS.Areas.Chat.ViewModels;
using HearUS.Application.Features.Chat.Commands.MarkChatRead;
using HearUS.Application.Features.Chat.Commands.StartConversation;
using HearUS.Application.Features.Chat.Queries.GetChatConversation;
using HearUS.Application.Features.Chat.Queries.GetChatInbox;
using HearUS.Application.Features.Chat.Queries.SearchChatUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HearUS.Areas.Chat.Controllers
{
    [Area("Chat")]
    [Route("Chat")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ISender _mediator;

        public HomeController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var inbox = await _mediator.Send(new GetChatInboxQuery(), cancellationToken);

            return View(new ChatIndexViewModel { Items = inbox.Items });
        }

        [HttpGet("New")]
        public async Task<IActionResult> New(string? q, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new SearchChatUsersQuery(q), cancellationToken);

            return View(new NewChatViewModel
            {
                Search = result.Search,
                Users = result.Users
            });
        }

        [HttpPost("Start")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Start(string userId, CancellationToken cancellationToken)
        {
            try
            {
                var conversationId = await _mediator.Send(new StartConversationCommand(userId), cancellationToken);

                return RedirectToAction(nameof(Conversation), new { id = conversationId });
            }
            catch (ValidationException exception)
            {
                TempData["Error"] = exception.Errors.First().ErrorMessage;
                return RedirectToAction(nameof(New));
            }
        }

        [HttpGet("Conversation/{id:int}")]
        public async Task<IActionResult> Conversation(int id, CancellationToken cancellationToken)
        {
            var conversation = await _mediator.Send(new GetChatConversationQuery(id), cancellationToken);

            await _mediator.Send(new MarkChatReadCommand(id), cancellationToken);

            return View(new ConversationViewModel
            {
                ConversationId = conversation.ConversationId,
                OtherName = conversation.OtherName,
                OtherInitial = conversation.OtherInitial,
                CurrentUserId = conversation.CurrentUserId,
                Messages = conversation.Messages
            });
        }
    }
}
