using HearUS.Areas.Jobs.ViewModels;
using HearUS.Application.Features.Jobs.Queries.GetActiveJobs;
using HearUS.Application.Features.Jobs.Queries.GetJobDetails;
using HearUS.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HearUS.Areas.Jobs.Controllers
{
    [Area("Jobs")]
    [Route("Jobs")]
    public class HomeController : Controller
    {
        private readonly ISender _mediator;

        public HomeController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string? search, int page = 1, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetActiveJobsQuery(search, page), cancellationToken);

            return View(new JobsIndexViewModel
            {
                Items = result.Items,
                Search = search,
                Pager = new PagerViewModel
                {
                    Page = result.Page,
                    TotalPages = (int)Math.Ceiling(result.TotalCount / (double)result.PageSize),
                    Area = "Jobs",
                    Controller = "Home",
                    Action = "Index",
                    RouteValues = string.IsNullOrWhiteSpace(search)
                        ? new Dictionary<string, string>()
                        : new Dictionary<string, string> { ["search"] = search }
                }
            });
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var job = await _mediator.Send(new GetJobDetailsQuery(id), cancellationToken);

            return View(job);
        }
    }
}
