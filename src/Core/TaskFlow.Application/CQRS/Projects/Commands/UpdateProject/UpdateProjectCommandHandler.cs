using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.ProjectDTOs;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.CQRS.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectDto?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateProjectCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectDto?> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects
                .Include(p => p.Members)
                .Include(p => p.CreatedBy)
                .Include(p => p.Tasks)
                .ThenInclude(t => t.AssignedTo)
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

            if (project == null)
                return null;

            var userId = request.UserId;

            if (project.CreatedByUserId != userId &&
                !project.Members.Any(m => m.UserId == userId && m.Role == "Owner"))
                return null;

            // Update properties
            project.Name = request.UpdateDto.Name;
            project.Description = request.UpdateDto.Description;
            project.StartDate = request.UpdateDto.StartDate;
            project.EndDate = request.UpdateDto.EndDate;
            project.Status = request.UpdateDto.Status;

            await _context.SaveChangesAsync(cancellationToken);

            // Map to DTO
            return _mapper.Map<ProjectDto>(project);
        }
    }

}
