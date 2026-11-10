using HearUS.Domain.Entities;

namespace HearUS.Application.Common.Interfaces.Repositories
{
    public interface ISignTermRepository
    {
        Task<(IReadOnlyList<SignTerm> Items, int TotalCount)> GetAsync(
            int page,
            int pageSize,
            string? search,
            SignCategory? category);

        Task<SignTerm?> GetByIdAsync(int id);

        Task AddAsync(SignTerm term);

        void Update(SignTerm term);
    }
}
