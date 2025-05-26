using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Project>> GetUserProjectsAsync(int userId)
        {
            return await _dbSet
                .Include(p => p.CreatedBy)
                .Include(p => p.Members)
                .Where(p => p.CreatedByUserId == userId ||
                           p.Members.Any(m => m.UserId == userId))
                .ToListAsync();
        }

        public async Task<List<Project>> GetProjectsWithTasksAsync()
        {
            return await _dbSet
                .Include(p => p.Tasks)
                .Include(p => p.CreatedBy)
                .ToListAsync();
        }
    }
}
