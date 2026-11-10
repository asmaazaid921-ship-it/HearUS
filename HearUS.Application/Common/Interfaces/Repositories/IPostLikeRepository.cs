using HearUS.Domain.Entities;

namespace HearUS.Application.Common.Interfaces.Repositories
{
    public interface IPostLikeRepository
    {
        Task<PostLike?> FindAsync(int postId, string userId);

        Task AddAsync(PostLike like);

        void Remove(PostLike like);

        Task<int> CountForPostAsync(int postId);
    }
}
