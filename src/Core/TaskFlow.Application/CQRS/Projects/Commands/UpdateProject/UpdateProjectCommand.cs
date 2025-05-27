using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.ProjectDTOs;

namespace TaskFlow.Application.CQRS.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommand : IRequest<ProjectDto?>
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public UpdateProjectDto UpdateDto { get; set; } = null!;
    }
}
