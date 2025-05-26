using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskFlow.API.DTOs.AuthDTOs;
using TaskFlow.API.DTOs.ProjectDTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context;

        public ProjectsController(IProjectRepository projectRepository, IUserRepository userRepository, AppDbContext context)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectDto>>> GetProjects()
        {
            var userId = GetCurrentUserId();
            var projects = await _projectRepository.GetUserProjectsAsync(userId);

            var projectDtos = projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Status = p.Status,
                CreatedAt = p.CreatedAt,
                CreatedBy = new UserDto
                {
                    Id = p.CreatedByUserId,
                    FirstName = p.CreatedBy.FirstName,
                    LastName = p.CreatedBy.LastName,
                    Email = p.CreatedBy.Email,
                    Role = p.CreatedBy.Role
                },
                TaskCount = p.Tasks.Count,
                CompletedTaskCount = p.Tasks.Count(t => t.Status == "Done"),
                Members = p.Members.Select(m => new ProjectMemberDto
                {
                    Id = m.Id,
                    UserId = m.UserId,
                    Role = m.Role,
                    JoinedAt = m.JoinedAt,
                    User = new UserDto
                    {
                        Id = m.User.Id,
                        FirstName = m.User.FirstName,
                        LastName = m.User.LastName,
                        Email = m.User.Email,
                        Role = m.User.Role
                    }
                }).ToList()
            }).ToList();

            return Ok(projectDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProject(int id)
        {
            var project = await _context.Projects
                .Include(p => p.CreatedBy)
                .Include(p => p.Tasks)
                .Include(p => p.Members)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return NotFound();

            // Check if user has access to this project
            var userId = GetCurrentUserId();
            if (project.CreatedByUserId != userId && !project.Members.Any(m => m.UserId == userId))
                return Forbid();

            var projectDto = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.Status,
                CreatedAt = project.CreatedAt,
                CreatedBy = new UserDto
                {
                    Id = project.CreatedBy.Id,
                    FirstName = project.CreatedBy.FirstName,
                    LastName = project.CreatedBy.LastName,
                    Email = project.CreatedBy.Email,
                    Role = project.CreatedBy.Role
                },
                TaskCount = project.Tasks.Count,
                CompletedTaskCount = project.Tasks.Count(t => t.Status == "Done"),
                Members = project.Members.Select(m => new ProjectMemberDto
                {
                    Id = m.Id,
                    UserId = m.UserId,
                    Role = m.Role,
                    JoinedAt = m.JoinedAt,
                    User = new UserDto
                    {
                        Id = m.User.Id,
                        FirstName = m.User.FirstName,
                        LastName = m.User.LastName,
                        Email = m.User.Email,
                        Role = m.User.Role
                    }
                }).ToList()
            };

            return Ok(projectDto);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> CreateProject(CreateProjectDto createProjectDto)
        {
            var userId = GetCurrentUserId();

            var project = new Project
            {
                Name = createProjectDto.Name,
                Description = createProjectDto.Description,
                StartDate = createProjectDto.StartDate,
                EndDate = createProjectDto.EndDate,
                CreatedByUserId = userId,
                Status = "Active"
            };

            await _projectRepository.AddAsync(project);

            // Add creator as project owner
            var projectMember = new ProjectMember
            {
                ProjectId = project.Id,
                UserId = userId,
                Role = "Owner"
            };

            _context.ProjectMembers.Add(projectMember);
            await _context.SaveChangesAsync();

            // Get the created project with includes
            var createdProject = await _context.Projects
                .Include(p => p.CreatedBy)
                .Include(p => p.Members)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(p => p.Id == project.Id);

            var projectDto = new ProjectDto
            {
                Id = createdProject.Id,
                Name = createdProject.Name,
                Description = createdProject.Description,
                StartDate = createdProject.StartDate,
                EndDate = createdProject.EndDate,
                Status = createdProject.Status,
                CreatedAt = createdProject.CreatedAt,
                CreatedBy = new UserDto
                {
                    Id = createdProject.CreatedBy.Id,
                    FirstName = createdProject.CreatedBy.FirstName,
                    LastName = createdProject.CreatedBy.LastName,
                    Email = createdProject.CreatedBy.Email,
                    Role = createdProject.CreatedBy.Role
                },
                TaskCount = 0,
                CompletedTaskCount = 0,
                Members = createdProject.Members.Select(m => new ProjectMemberDto
                {
                    Id = m.Id,
                    UserId = m.UserId,
                    Role = m.Role,
                    JoinedAt = m.JoinedAt,
                    User = new UserDto
                    {
                        Id = m.User.Id,
                        FirstName = m.User.FirstName,
                        LastName = m.User.LastName,
                        Email = m.User.Email,
                        Role = m.User.Role
                    }
                }).ToList()
            };

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, projectDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectDto>> UpdateProject(int id, UpdateProjectDto updateProjectDto)
        {
            var project = await _context.Projects
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return NotFound();

            var userId = GetCurrentUserId();

            // Check if user is owner or has admin role
            if (project.CreatedByUserId != userId && !project.Members.Any(m => m.UserId == userId && m.Role == "Owner"))
                return Forbid();

            project.Name = updateProjectDto.Name;
            project.Description = updateProjectDto.Description;
            project.StartDate = updateProjectDto.StartDate;
            project.EndDate = updateProjectDto.EndDate;
            project.Status = updateProjectDto.Status;

            await _projectRepository.UpdateAsync(project);

            return await GetProject(id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return NotFound();

            var userId = GetCurrentUserId();

            // Only creator can delete project
            if (project.CreatedByUserId != userId)
                return Forbid();

            await _projectRepository.DeleteAsync(id);

            return NoContent();
        }

        [HttpPost("{id}/members")]
        public async Task<ActionResult> AddMember(int id, AddMemberDto addMemberDto)
        {
            var project = await _context.Projects
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return NotFound();

            var userId = GetCurrentUserId();

            // Check if user is owner
            if (project.CreatedByUserId != userId && !project.Members.Any(m => m.UserId == userId && m.Role == "Owner"))
                return Forbid();

            // Check if user already member
            if (project.Members.Any(m => m.UserId == addMemberDto.UserId))
                return BadRequest("User is already a member of this project");

            var member = new ProjectMember
            {
                ProjectId = id,
                UserId = addMemberDto.UserId,
                Role = addMemberDto.Role
            };

            _context.ProjectMembers.Add(member);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }
    }
}
