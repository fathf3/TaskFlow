using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskFlow.Application.CQRS.Projects.Commands.AddProjectMember;
using TaskFlow.Application.CQRS.Projects.Commands.CreateProject;
using TaskFlow.Application.CQRS.Projects.Commands.DeleteProject;
using TaskFlow.Application.CQRS.Projects.Commands.UpdateProject;
using TaskFlow.Application.CQRS.Projects.Queries;
using TaskFlow.Application.CQRS.Projects.Queries.GetProjectById;
using TaskFlow.Application.DTOs.ProjectDTOs;

namespace TaskFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
       
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
           
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectDto>>> GetProjects()
        {
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(new GetUserProjectsQuery(userId));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var userId = GetCurrentUserId(); // Bu metot kullanıcı ID'sini almalı

            var projectDto = await _mediator.Send(new GetProjectByIdQuery(id, userId));

            if (projectDto == null)
                return Forbid(); // veya NotFound() -- isteğe göre

            return Ok(projectDto);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> CreateProject(CreateProjectDto createProjectDto)
        {
            var userId = GetCurrentUserId();

            var command = new CreateProjectCommand
            {
                Name = createProjectDto.Name,
                Description = createProjectDto.Description,
                StartDate = createProjectDto.StartDate,
                EndDate = createProjectDto.EndDate,
                CreatedByUserId = userId
            };

            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetProject), new { id = result.Id }, result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, UpdateProjectDto updateProjectDto)
        {
            var userId = GetCurrentUserId();

            var result = await _mediator.Send(new UpdateProjectCommand
            {
                ProjectId = id,
                UserId = userId,
                UpdateDto = updateProjectDto
            });

            if (result == null)
                return Forbid();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var userId = GetCurrentUserId();

            var success = await _mediator.Send(new DeleteProjectCommand(id, userId));

            if (!success)
                return Forbid();

            return NoContent();
        }

        [HttpPost("{id}/members")]
        public async Task<IActionResult> AddMember(int id, AddMemberDto addMemberDto)
        {
            var result = await _mediator.Send(new AddProjectMemberCommand
            {
                ProjectId = id,
                RequestingUserId = GetCurrentUserId(),
                MemberDto = addMemberDto
            });

            if (!result)
                return Forbid();

            return Ok();
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }
    }
}
