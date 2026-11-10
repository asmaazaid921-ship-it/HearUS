using HearUS.Infrastructure.Persistence;
using HearUS.Domain.Entities;
using HearUS.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HearUS.Infrastructure.Persistence.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Comment>> GetForPostAsync(int postId)
        {
            return await _context.Comments
                .AsNoTracking()
                .Include(c => c.Author)
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        public Task AddAsync(Comment comment)
        {
            return _context.Comments.AddAsync(comment).AsTask();
        }

        public void Remove(Comment comment)
        {
            _context.Comments.Remove(comment);
        }
    }
}
