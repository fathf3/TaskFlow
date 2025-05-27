using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.ProjectDTOs;

namespace TaskFlow.Application.CQRS.Projects.Queries.GetProjectById
{
    public class GetProjectByIdQuery : IRequest<ProjectDto?>
    {
        public int ProjectId { get; }
        public int UserId { get; }

        public GetProjectByIdQuery(int projectId, int userId)
        {
            ProjectId = projectId;
            UserId = userId;
        }
    }
}
