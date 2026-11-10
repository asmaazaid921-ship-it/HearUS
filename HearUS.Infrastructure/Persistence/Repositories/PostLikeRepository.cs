using HearUS.Infrastructure.Persistence;
using HearUS.Domain.Entities;
using HearUS.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HearUS.Infrastructure.Persistence.Repositories
{
    public class PostLikeRepository : IPostLikeRepository
    {
        private readonly ApplicationDbContext _context;

        public PostLikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<PostLike?> FindAsync(int postId, string userId)
        {
            return _context.PostLikes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
        }

        public Task AddAsync(PostLike like)
        {
            return _context.PostLikes.AddAsync(like).AsTask();
        }

        public void Remove(PostLike like)
        {
            _context.PostLikes.Remove(like);
        }

        public Task<int> CountForPostAsync(int postId)
        {
            return _context.PostLikes.CountAsync(l => l.PostId == postId);
        }
    }
}
