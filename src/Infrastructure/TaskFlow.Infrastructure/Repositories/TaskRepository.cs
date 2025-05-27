using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories
{
    public class TaskRepository : Repository<TaskItem>, ITaskRepository
    {
        public TaskRepository(AppDbContext context) : base(context)
        {
        }

        public Task<List<TaskItem>> GetTasksByProjectId(int projectId)
        {
           return _dbSet.Where(task => task.ProjectId == projectId)
                        .Include(task => task.Project)
                        .ToListAsync();
        }
    }
}
