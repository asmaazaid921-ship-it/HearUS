using HearUS.Domain.Entities;

namespace HearUS.Application.Common.Interfaces.Repositories
{
    public interface IJobRepository
    {
        Task<(IReadOnlyList<Job> Items, int TotalCount)> GetActiveAsync(
            int page,
            int pageSize,
            string? search = null);

        Task<(IReadOnlyList<Job> Items, int TotalCount)> GetAllAsync(
            int page,
            int pageSize,
            string? search = null);

        Task<Job?> GetByIdAsync(int id);

        Task AddAsync(Job job);

        void Update(Job job);
    }
}
