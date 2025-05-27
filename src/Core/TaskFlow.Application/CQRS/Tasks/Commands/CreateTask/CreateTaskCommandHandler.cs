using AutoMapper;
using MediatR;
using TaskFlow.Application.DTOs.TaskDTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.CQRS.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public CreateTaskCommandHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var taskItem = _mapper.Map<TaskItem>(request);
            taskItem.CreatedAt = DateTime.UtcNow;

            await _taskRepository.AddAsync(taskItem);
            return _mapper.Map<TaskDto>(taskItem);
        }
    }
}
