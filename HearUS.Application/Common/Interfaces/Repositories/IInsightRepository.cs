using HearUS.Domain.Entities;

namespace HearUS.Application.Common.Interfaces.Repositories
{
    public interface IInsightRepository
    {
        Task<(IReadOnlyList<Insight> Items, int TotalCount)> GetPublishedAsync(
            int page,
            int pageSize,
            InsightCategory? category = null);

        Task<(IReadOnlyList<Insight> Items, int TotalCount)> GetForAuthorAsync(
            string authorId,
            int page,
            int pageSize);

        Task<Insight?> GetByIdAsync(int id);

        Task AddAsync(Insight insight);

        void Update(Insight insight);
    }
}
