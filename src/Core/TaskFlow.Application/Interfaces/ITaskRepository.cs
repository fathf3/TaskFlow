using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces
{
    public interface ITaskRepository : IRepository<TaskItem>
    {
        Task<List<TaskItem>> GetTasksByProjectId(int projectId);
    }
}
