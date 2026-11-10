using HearUS.Domain.Entities;

namespace HearUS.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByIdAsync(string id);

        Task<IReadOnlyList<ApplicationUser>> SearchAsync(string? term, string excludeUserId, int take = 20);

        Task<IReadOnlySet<string>> GetUserIdsInRoleAsync(string roleName);
    }
}
