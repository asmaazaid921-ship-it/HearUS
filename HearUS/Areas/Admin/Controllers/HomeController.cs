using HearUS.Areas.Admin.ViewModels;
using HearUS.Application.Features.Admin.Queries.GetAdminDashboard;
using HearUS.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HearUS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppRoles.Admin)]
    public class HomeController : Controller
    {
        private readonly ISender _mediator;

        public HomeController(ISender mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var dashboard = await _mediator.Send(new GetAdminDashboardQuery(), cancellationToken);

            return View(new AdminDashboardViewModel
            {
                UsersCount = dashboard.UsersCount,
                PostsCount = dashboard.PostsCount,
                PublishedInsightsCount = dashboard.PublishedInsightsCount,
                ActiveJobsCount = dashboard.ActiveJobsCount,
                LatestJobs = dashboard.LatestJobs
            });
        }
    }
}
