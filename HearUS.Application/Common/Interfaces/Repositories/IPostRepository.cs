using HearUS.Domain.Entities;

namespace HearUS.Application.Common.Interfaces.Repositories
{
    public interface IPostRepository
    {
        Task<(IReadOnlyList<Post> Items, int TotalCount)> GetFeedAsync(int page, int pageSize);

        Task<Post?> GetByIdAsync(int id);

        Task<IReadOnlySet<int>> GetLikedPostIdsAsync(string userId, IEnumerable<int> postIds);

        Task<int> CountAsync();

        Task<int> CountForUserAsync(string userId);

        Task AddAsync(Post post);

        void Update(Post post);
    }
}
