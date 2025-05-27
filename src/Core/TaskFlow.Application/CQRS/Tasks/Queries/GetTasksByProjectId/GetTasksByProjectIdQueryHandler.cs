using AutoMapper;
using MediatR;
using TaskFlow.Application.DTOs.TaskDTOs;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.CQRS.Tasks.Queries.GetTasksByProjectId
{
    public class GetTasksByProjectIdQueryHandler : IRequestHandler<GetTasksByProjectIdQuery, List<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GetTasksByProjectIdQueryHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<List<TaskDto>> Handle(GetTasksByProjectIdQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetTasksByProjectId(request.ProjectId);
            return _mapper.Map<List<TaskDto>>(tasks);
        }
    }
}
