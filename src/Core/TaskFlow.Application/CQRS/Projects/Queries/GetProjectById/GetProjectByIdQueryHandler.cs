using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs.ProjectDTOs;
using TaskFlow.Application.DTOs.UserDTOs;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.CQRS.Projects.Queries.GetProjectById
{
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProjectByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects
                .Include(p => p.CreatedBy)
                .Include(p => p.Tasks)
                .Include(p => p.Members)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

            if (project == null)
                return null;

            // Kullanıcıya erişim izni kontrolü
            if (project.CreatedByUserId != request.UserId &&
                !project.Members.Any(m => m.UserId == request.UserId))
            {
                // Yetkisiz ise null döndür. (Alternatif: özel hata fırlat)
                return null;
            }

            var projectDto = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.Status,
                CreatedAt = project.CreatedAt,
                CreatedBy = _mapper.Map<UserDto>(project.CreatedBy),
                TaskCount = project.Tasks.Count,
                CompletedTaskCount = project.Tasks.Count(t => t.Status == "Done"),
                Members = project.Members.Select(m => new ProjectMemberDto
                {
                    Id = m.Id,
                    UserId = m.UserId,
                    Role = m.Role,
                    JoinedAt = m.JoinedAt,
                    User = _mapper.Map<UserDto>(m.User)
                }).ToList()
            };

            return projectDto;
        }
    }
}
