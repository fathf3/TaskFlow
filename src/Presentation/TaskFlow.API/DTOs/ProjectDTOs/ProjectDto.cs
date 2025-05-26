using TaskFlow.API.DTOs.AuthDTOs;

namespace TaskFlow.API.DTOs.ProjectDTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDto CreatedBy { get; set; }
        public int TaskCount { get; set; }
        public int CompletedTaskCount { get; set; }
        public List<ProjectMemberDto> Members { get; set; } = new List<ProjectMemberDto>();
    }
}
