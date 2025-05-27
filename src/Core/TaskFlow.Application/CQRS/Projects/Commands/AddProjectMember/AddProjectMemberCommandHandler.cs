using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.CQRS.Projects.Commands.AddProjectMember
{
    public class AddProjectMemberCommandHandler : IRequestHandler<AddProjectMemberCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AddProjectMemberCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(AddProjectMemberCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

            if (project == null)
                return false;

            var userId = request.RequestingUserId;

            var isOwner = project.CreatedByUserId == userId ||
                          project.Members.Any(m => m.UserId == userId && m.Role == "Owner");

            if (!isOwner)
                return false;

            // Already a member?
            if (project.Members.Any(m => m.UserId == request.MemberDto.UserId))
                throw new Exception("User is already a member of this project");

            var newMember = _mapper.Map<ProjectMember>(request.MemberDto);
            newMember.ProjectId = request.ProjectId;

            _context.ProjectMembers.Add(newMember);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }

}
