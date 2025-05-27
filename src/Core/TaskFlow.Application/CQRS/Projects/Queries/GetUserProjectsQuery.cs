using MediatR;
using TaskFlow.Application.DTOs.ProjectDTOs;


namespace TaskFlow.Application.CQRS.Projects.Queries
{
    public record GetUserProjectsQuery(int UserId) : IRequest<List<ProjectDto>>;

}
