using HearUS.Infrastructure.Persistence;
using HearUS.Domain.Entities;
using HearUS.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HearUS.Infrastructure.Persistence.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IReadOnlyList<Post> Items, int TotalCount)> GetFeedAsync(int page, int pageSize)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var query = _context.Posts
                .AsNoTracking()
                .Include(p => p.Author)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Author)
                .Include(p => p.Likes)
                .OrderByDescending(p => p.CreatedAt);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public Task<Post?> GetByIdAsync(int id)
        {
            return _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Author)
                .Include(p => p.Likes)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task AddAsync(Post post)
        {
            return _context.Posts.AddAsync(post).AsTask();
        }

        public async Task<IReadOnlySet<int>> GetLikedPostIdsAsync(string userId, IEnumerable<int> postIds)
        {
            var ids = postIds.Distinct().ToList();
            if (ids.Count == 0)
            {
                return new HashSet<int>();
            }

            var likedIds = await _context.PostLikes
                .AsNoTracking()
                .Where(l => l.UserId == userId && ids.Contains(l.PostId))
                .Select(l => l.PostId)
                .ToListAsync();

            return likedIds.ToHashSet();
        }

        public Task<int> CountAsync()
        {
            return _context.Posts.CountAsync();
        }

        public void Update(Post post)
        {
            _context.Posts.Update(post);
        }

        public Task<int> CountForUserAsync(string userId)
        {
            return _context.Posts.CountAsync(p => p.AuthorId == userId);
        }
    }
}
