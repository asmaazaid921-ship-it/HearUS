using FluentValidation;
using HearUS.Areas.Signs.ViewModels;
using HearUS.Application.Features.Signs.Commands.CreateSignTerm;
using HearUS.Application.Features.Signs.Commands.UpdateSignTerm;
using HearUS.Application.Features.Signs.Queries.GetSignTermForEdit;
using HearUS.Domain.Constants;
using HearUS.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HearUS.Areas.Signs.Controllers
{
    [Area("Signs")]
    [Route("Signs")]
    [Authorize(Policy = AppPolicies.ExpertsOnly)]
    public class TermsController : Controller
    {
        private readonly ISender _mediator;

        public TermsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View(new SignFormViewModel());
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SignFormViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _mediator.Send(new CreateSignTermCommand(
                    model.Term,
                    model.Description,
                    model.Category,
                    model.VideoUrl), cancellationToken);
            }
            catch (ValidationException exception)
            {
                ModelState.AddValidationErrors(exception);
                return View(model);
            }

            TempData["Success"] = "تمت إضافة الإشارة إلى القاموس.";
            return RedirectToAction("Index", "Home", new { area = "Signs" });
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var term = await _mediator.Send(new GetSignTermForEditQuery(id), cancellationToken);

            return View(new SignFormViewModel
            {
                Id = term.Id,
                Term = term.Term,
                Description = term.Description,
                Category = term.Category,
                VideoUrl = term.VideoUrl
            });
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SignFormViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await _mediator.Send(new GetSignTermForEditQuery(id), cancellationToken);
                model.Id = id;
                return View(model);
            }

            try
            {
                await _mediator.Send(new UpdateSignTermCommand(
                    id,
                    model.Term,
                    model.Description,
                    model.Category,
                    model.VideoUrl), cancellationToken);
            }
            catch (ValidationException exception)
            {
                model.Id = id;
                ModelState.AddValidationErrors(exception);
                return View(model);
            }

            TempData["Success"] = "تم تحديث الإشارة.";
            return RedirectToAction("Details", "Home", new { area = "Signs", id });
        }
    }
}
