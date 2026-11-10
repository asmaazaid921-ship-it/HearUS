using HearUS.Infrastructure.Persistence;
using HearUS.Domain.Entities;
using HearUS.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HearUS.Infrastructure.Persistence.Repositories
{
    public class InsightRepository : IInsightRepository
    {
        private readonly ApplicationDbContext _context;

        public InsightRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IReadOnlyList<Insight> Items, int TotalCount)> GetPublishedAsync(
            int page,
            int pageSize,
            InsightCategory? category = null)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var query = _context.Insights
                .AsNoTracking()
                .Include(i => i.Author)
                .Where(i => i.IsPublished);

            if (category.HasValue)
            {
                query = query.Where(i => i.Category == category.Value);
            }

            var ordered = query.OrderByDescending(i => i.PublishedAt ?? i.CreatedAt);
            var totalCount = await ordered.CountAsync();
            var items = await ordered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(IReadOnlyList<Insight> Items, int TotalCount)> GetForAuthorAsync(
            string authorId,
            int page,
            int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var query = _context.Insights
                .AsNoTracking()
                .Include(i => i.Author)
                .Where(i => i.AuthorId == authorId)
                .OrderByDescending(i => i.CreatedAt);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public Task<Insight?> GetByIdAsync(int id)
        {
            return _context.Insights
                .Include(i => i.Author)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public Task AddAsync(Insight insight)
        {
            return _context.Insights.AddAsync(insight).AsTask();
        }

        public void Update(Insight insight)
        {
            _context.Insights.Update(insight);
        }
    }
}
