using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.CQRS.Tasks.Commands.CreateTask;
using TaskFlow.Application.CQRS.Tasks.Queries.GetTasksByProjectId;
using TaskFlow.Application.DTOs.TaskDTOs;

namespace TaskFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskDto>>> GetByProjectId([FromQuery] int projectId)
        {
            var result = await _mediator.Send(new GetTasksByProjectIdQuery(projectId));
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> Create(CreateTaskCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetById(int id)
        {
            var result = await _mediator.Send(new GetTasksByProjectIdQuery(id));
            return result == null ? NotFound() : Ok(result);
        }

        // Update & Delete metodları da eklenebilir.
    }
}
