using HearUS.Infrastructure.Persistence;
using HearUS.Domain.Entities;
using HearUS.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HearUS.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IReadOnlyList<ApplicationUser>> SearchAsync(string? term, string excludeUserId, int take = 20)
        {
            var query = _context.Users
                .AsNoTracking()
                .Where(u => u.Id != excludeUserId && u.IsActive);

            if (!string.IsNullOrWhiteSpace(term))
            {
                var value = term.Trim();
                query = query.Where(u => u.FullName.Contains(value) || (u.Email != null && u.Email.Contains(value)));
            }

            return await query
                .OrderBy(u => u.FullName)
                .Take(Math.Clamp(take, 1, 50))
                .ToListAsync();
        }

        public async Task<IReadOnlySet<string>> GetUserIdsInRoleAsync(string roleName)
        {
            var userIds = await (
                from userRole in _context.UserRoles
                join role in _context.Roles on userRole.RoleId equals role.Id
                where role.Name == roleName
                select userRole.UserId)
                .ToListAsync();

            return userIds.ToHashSet();
        }
    }
}
