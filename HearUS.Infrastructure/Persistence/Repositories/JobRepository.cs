using HearUS.Infrastructure.Persistence;
using HearUS.Domain.Entities;
using HearUS.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HearUS.Infrastructure.Persistence.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _context;

        public JobRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IReadOnlyList<Job> Items, int TotalCount)> GetActiveAsync(
            int page,
            int pageSize,
            string? search = null)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var query = _context.Jobs
                .AsNoTracking()
                .Where(j => j.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(j =>
                    j.Title.Contains(term) ||
                    j.CompanyName.Contains(term) ||
                    (j.Location != null && j.Location.Contains(term)));
            }

            var ordered = query.OrderByDescending(j => j.PostedAt);
            var totalCount = await ordered.CountAsync();
            var items = await ordered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(IReadOnlyList<Job> Items, int TotalCount)> GetAllAsync(
            int page,
            int pageSize,
            string? search = null)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var query = _context.Jobs.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(j =>
                    j.Title.Contains(term) ||
                    j.CompanyName.Contains(term) ||
                    (j.Location != null && j.Location.Contains(term)));
            }

            var ordered = query.OrderByDescending(j => j.PostedAt);
            var totalCount = await ordered.CountAsync();
            var items = await ordered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public Task<Job?> GetByIdAsync(int id)
        {
            return _context.Jobs.FirstOrDefaultAsync(j => j.Id == id);
        }

        public Task AddAsync(Job job)
        {
            return _context.Jobs.AddAsync(job).AsTask();
        }

        public void Update(Job job)
        {
            _context.Jobs.Update(job);
        }
    }
}
