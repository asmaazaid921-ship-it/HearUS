using HearUS.Areas.Signs.ViewModels;
using HearUS.Application.Features.Signs.Queries.GetSignTermDetails;
using HearUS.Application.Features.Signs.Queries.GetSignTerms;
using HearUS.Domain.Entities;
using HearUS.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HearUS.Areas.Signs.Controllers
{
    [Area("Signs")]
    [Route("Signs")]
    public class HomeController : Controller
    {
        private readonly ISender _mediator;

        public HomeController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(
            string? search,
            SignCategory? category,
            int page = 1,
            CancellationToken cancellationToken = default)
        {
            var searchTerm = string.IsNullOrWhiteSpace(search) ? null : search.Trim();
            var result = await _mediator.Send(new GetSignTermsQuery(searchTerm, category, page), cancellationToken);

            var routeValues = new Dictionary<string, string>();
            if (searchTerm is not null)
            {
                routeValues["search"] = searchTerm;
            }

            if (category.HasValue)
            {
                routeValues["category"] = category.Value.ToString();
            }

            return View(new SignsIndexViewModel
            {
                Items = result.Items,
                Search = searchTerm,
                Category = category,
                CanManage = result.CanManage,
                Pager = new PagerViewModel
                {
                    Page = result.Page,
                    TotalPages = (int)Math.Ceiling(result.TotalCount / (double)result.PageSize),
                    Area = "Signs",
                    Controller = "Home",
                    Action = "Index",
                    RouteValues = routeValues
                }
            });
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var term = await _mediator.Send(new GetSignTermDetailsQuery(id), cancellationToken);

            return View(term);
        }
    }
}
