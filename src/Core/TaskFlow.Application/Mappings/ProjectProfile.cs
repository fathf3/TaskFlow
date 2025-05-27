using AutoMapper;
using TaskFlow.Application.CQRS.Tasks.Commands.CreateTask;
using TaskFlow.Application.DTOs.ProjectDTOs;
using TaskFlow.Application.DTOs.TaskDTOs;
using TaskFlow.Application.DTOs.UserDTOs;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Mappings
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.TaskCount, opt => opt.MapFrom(src => src.Tasks.Count))
                .ForMember(dest => dest.CompletedTaskCount, opt => opt.MapFrom(src => src.Tasks.Count(t => t.Status == "Done")));

            CreateMap<User, UserDto>();
            CreateMap<ProjectMember, ProjectMemberDto>()
           .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
            CreateMap<AddMemberDto, ProjectMember>();
            
            CreateMap<TaskItem, TaskDto>().ReverseMap();
            CreateMap<CreateTaskCommand, TaskItem>(); 



        }
    }
}
