using FluentValidation;
using HearUS.Areas.Admin.ViewModels;
using HearUS.Application.Features.Jobs.Commands.CreateJob;
using HearUS.Application.Features.Jobs.Commands.ToggleJobActive;
using HearUS.Application.Features.Jobs.Commands.UpdateJob;
using HearUS.Application.Features.Jobs.Queries.GetAdminJobs;
using HearUS.Application.Features.Jobs.Queries.GetJobForEdit;
using HearUS.Domain.Constants;
using HearUS.ViewModels;
using HearUS.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HearUS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppRoles.Admin)]
    public class JobsController : Controller
    {
        private readonly ISender _mediator;

        public JobsController(ISender mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(string? search, int page = 1, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAdminJobsQuery(search, page), cancellationToken);

            return View(new AdminJobsViewModel
            {
                Items = result.Items,
                Search = search,
                Pager = new PagerViewModel
                {
                    Page = result.Page,
                    TotalPages = (int)Math.Ceiling(result.TotalCount / (double)result.PageSize),
                    Area = "Admin",
                    Controller = "Jobs",
                    Action = "Index",
                    RouteValues = string.IsNullOrWhiteSpace(search)
                        ? new Dictionary<string, string>()
                        : new Dictionary<string, string> { ["search"] = search }
                }
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new JobFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobFormViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _mediator.Send(new CreateJobCommand(
                    model.Title,
                    model.CompanyName,
                    model.Description,
                    model.SignVideoUrl,
                    model.Location,
                    model.IsRemote,
                    model.IsAccessibilityFriendly,
                    model.ApplyUrl,
                    model.ContactEmail,
                    model.IsActive), cancellationToken);
            }
            catch (ValidationException exception)
            {
                ModelState.AddValidationErrors(exception);
                return View(model);
            }

            TempData["Success"] = "تمت إضافة الوظيفة.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var job = await _mediator.Send(new GetJobForEditQuery(id), cancellationToken);

            return View(new JobFormViewModel
            {
                Id = job.Id,
                Title = job.Title,
                CompanyName = job.CompanyName,
                Description = job.Description,
                SignVideoUrl = job.SignVideoUrl,
                Location = job.Location,
                IsRemote = job.IsRemote,
                IsAccessibilityFriendly = job.IsAccessibilityFriendly,
                ApplyUrl = job.ApplyUrl,
                ContactEmail = job.ContactEmail,
                IsActive = job.IsActive
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JobFormViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await _mediator.Send(new GetJobForEditQuery(id), cancellationToken);
                model.Id = id;
                return View(model);
            }

            try
            {
                await _mediator.Send(new UpdateJobCommand(
                    id,
                    model.Title,
                    model.CompanyName,
                    model.Description,
                    model.SignVideoUrl,
                    model.Location,
                    model.IsRemote,
                    model.IsAccessibilityFriendly,
                    model.ApplyUrl,
                    model.ContactEmail,
                    model.IsActive), cancellationToken);
            }
            catch (ValidationException exception)
            {
                model.Id = id;
                ModelState.AddValidationErrors(exception);
                return View(model);
            }

            TempData["Success"] = "تم تحديث الوظيفة.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id, CancellationToken cancellationToken)
        {
            var isActive = await _mediator.Send(new ToggleJobActiveCommand(id), cancellationToken);

            TempData["Success"] = isActive ? "تم نشر الوظيفة." : "تم إيقاف نشر الوظيفة.";
            return RedirectToAction(nameof(Index));
        }
    }
}
