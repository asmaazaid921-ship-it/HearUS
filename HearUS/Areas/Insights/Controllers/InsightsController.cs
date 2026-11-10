using FluentValidation;
using HearUS.Areas.Insights.ViewModels;
using HearUS.Application.Features.Insights.Commands.CreateInsight;
using HearUS.Application.Features.Insights.Commands.UpdateInsight;
using HearUS.Application.Features.Insights.Queries.GetInsightForEdit;
using HearUS.Application.Features.Insights.Queries.GetMyInsights;
using HearUS.Domain.Constants;
using HearUS.ViewModels;
using HearUS.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HearUS.Areas.Insights.Controllers
{
    [Area("Insights")]
    [Route("Insights")]
    [Authorize(Policy = AppPolicies.ExpertsOnly)]
    public class InsightsController : Controller
    {
        private readonly ISender _mediator;

        public InsightsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Write")]
        public IActionResult Create()
        {
            return View(new InsightFormViewModel());
        }

        [HttpPost("Write")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsightFormViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _mediator.Send(new CreateInsightCommand(
                    model.Title,
                    model.Summary,
                    model.Content,
                    model.SignVideoUrl,
                    model.Category,
                    model.IsPublished), cancellationToken);
            }
            catch (ValidationException exception)
            {
                ModelState.AddValidationErrors(exception);
                return View(model);
            }

            TempData["Success"] = model.IsPublished
                ? "تم نشر مقالك في ركن صوتهم."
                : "تم حفظ المقال كمسودة.";

            return RedirectToAction(nameof(My));
        }

        [HttpGet("My")]
        public async Task<IActionResult> My(int page = 1, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetMyInsightsQuery(page), cancellationToken);

            return View(new MyInsightsViewModel
            {
                Items = result.Items,
                Pager = new PagerViewModel
                {
                    Page = result.Page,
                    TotalPages = (int)Math.Ceiling(result.TotalCount / (double)result.PageSize),
                    Area = "Insights",
                    Controller = "Insights",
                    Action = "My"
                }
            });
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var insight = await _mediator.Send(new GetInsightForEditQuery(id), cancellationToken);

            return View(new InsightFormViewModel
            {
                Id = insight.Id,
                Title = insight.Title,
                Summary = insight.Summary,
                Content = insight.Content,
                SignVideoUrl = insight.SignVideoUrl,
                Category = insight.Category,
                IsPublished = insight.IsPublished
            });
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            InsightFormViewModel model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await _mediator.Send(new GetInsightForEditQuery(id), cancellationToken);
                model.Id = id;
                return View(model);
            }

            try
            {
                await _mediator.Send(new UpdateInsightCommand(
                    id,
                    model.Title,
                    model.Summary,
                    model.Content,
                    model.SignVideoUrl,
                    model.Category,
                    model.IsPublished), cancellationToken);
            }
            catch (ValidationException exception)
            {
                model.Id = id;
                ModelState.AddValidationErrors(exception);
                return View(model);
            }

            TempData["Success"] = "تم تحديث المقال.";
            return RedirectToAction(nameof(My));
        }
    }
}
