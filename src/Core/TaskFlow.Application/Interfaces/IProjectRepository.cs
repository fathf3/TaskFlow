using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<List<Project>> GetUserProjectsAsync(int userId);
        Task<List<Project>> GetProjectsWithTasksAsync();
    }
}
