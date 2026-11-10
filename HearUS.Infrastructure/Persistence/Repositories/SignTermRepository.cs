using HearUS.Infrastructure.Persistence;
using HearUS.Domain.Entities;
using HearUS.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HearUS.Infrastructure.Persistence.Repositories
{
    public class SignTermRepository : ISignTermRepository
    {
        private readonly ApplicationDbContext _context;

        public SignTermRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IReadOnlyList<SignTerm> Items, int TotalCount)> GetAsync(
            int page,
            int pageSize,
            string? search,
            SignCategory? category)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var query = _context.SignTerms
                .AsNoTracking()
                .Include(t => t.CreatedBy)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(t => t.Term.Contains(term) ||
                                         (t.Description != null && t.Description.Contains(term)));
            }

            if (category.HasValue)
            {
                query = query.Where(t => t.Category == category.Value);
            }

            var ordered = query.OrderBy(t => t.Term);
            var totalCount = await ordered.CountAsync();
            var items = await ordered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public Task<SignTerm?> GetByIdAsync(int id)
        {
            return _context.SignTerms
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public Task AddAsync(SignTerm term)
        {
            return _context.SignTerms.AddAsync(term).AsTask();
        }

        public void Update(SignTerm term)
        {
            _context.SignTerms.Update(term);
        }
    }
}
