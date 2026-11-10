using HearUS.Domain.Entities;

namespace HearUS.Application.Common.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<IReadOnlyList<Comment>> GetForPostAsync(int postId);

        Task AddAsync(Comment comment);

        void Remove(Comment comment);
    }
}
