using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.TaskDTOs;

namespace TaskFlow.Application.CQRS.Tasks.Queries.GetTasksByProjectId
{
    public class GetTasksByProjectIdQuery : IRequest<List<TaskDto>>
    {
        public int ProjectId { get; set; }

        public GetTasksByProjectIdQuery(int projectId)
        {
            ProjectId = projectId;
        }
    }
}
