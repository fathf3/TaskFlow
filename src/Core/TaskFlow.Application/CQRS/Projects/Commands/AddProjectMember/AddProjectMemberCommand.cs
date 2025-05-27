using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.ProjectDTOs;

namespace TaskFlow.Application.CQRS.Projects.Commands.AddProjectMember
{
    public class AddProjectMemberCommand : IRequest<bool>
    {
        public int ProjectId { get; set; }
        public int RequestingUserId { get; set; }
        public AddMemberDto MemberDto { get; set; } = null!;
    }
}
