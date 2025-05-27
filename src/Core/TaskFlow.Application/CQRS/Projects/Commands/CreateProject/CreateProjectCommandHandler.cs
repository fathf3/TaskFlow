using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.ProjectDTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.CQRS.Projects.Commands.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateProjectCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Project
            {
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                CreatedByUserId = request.CreatedByUserId,
                Status = "Active"
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync(cancellationToken);

            var projectMember = new ProjectMember
            {
                ProjectId = project.Id,
                UserId = request.CreatedByUserId,
                Role = "Owner"
            };

            _context.ProjectMembers.Add(projectMember);
            await _context.SaveChangesAsync(cancellationToken);

            var createdProject = await _context.Projects
                .Include(p => p.CreatedBy)
                .Include(p => p.Members)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(p => p.Id == project.Id, cancellationToken);

            return _mapper.Map<ProjectDto>(createdProject);
        }
    }
}