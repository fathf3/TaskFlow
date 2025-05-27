using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.ProjectDTOs;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.CQRS.Projects.Queries
{
    public class GetUserProjectsQueryHandler : IRequestHandler<GetUserProjectsQuery, List<ProjectDto>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public GetUserProjectsQueryHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<List<ProjectDto>> Handle(GetUserProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _projectRepository.GetUserProjectsAsync(request.UserId);
            return _mapper.Map<List<ProjectDto>>(projects);
        }
    }

}
