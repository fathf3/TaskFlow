using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.CQRS.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommand : IRequest<bool>
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }

        public DeleteProjectCommand(int projectId, int userId)
        {
            ProjectId = projectId;
            UserId = userId;
        }
    }
}
