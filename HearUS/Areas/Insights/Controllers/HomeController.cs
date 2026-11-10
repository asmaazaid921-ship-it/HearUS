using HearUS.Areas.Insights.ViewModels;
using HearUS.Application.Features.Insights.Queries.GetInsightDetails;
using HearUS.Application.Features.Insights.Queries.GetPublishedInsights;
using HearUS.Domain.Entities;
using HearUS.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HearUS.Areas.Insights.Controllers
{
    [Area("Insights")]
    [Route("Insights")]
    public class HomeController : Controller
    {
        private readonly ISender _mediator;

        public HomeController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(
            int page = 1,
            InsightCategory? category = null,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetPublishedInsightsQuery(page, category), cancellationToken);

            return View(new InsightsIndexViewModel
            {
                Items = result.Items,
                Category = category,
                CanWrite = result.CanWrite,
                Pager = new PagerViewModel
                {
                    Page = result.Page,
                    TotalPages = (int)Math.Ceiling(result.TotalCount / (double)result.PageSize),
                    Area = "Insights",
                    Controller = "Home",
                    Action = "Index",
                    RouteValues = category.HasValue
                        ? new Dictionary<string, string> { ["category"] = category.Value.ToString() }
                        : new Dictionary<string, string>()
                }
            });
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var insight = await _mediator.Send(new GetInsightDetailsQuery(id), cancellationToken);

            return View(insight);
        }
    }
}
