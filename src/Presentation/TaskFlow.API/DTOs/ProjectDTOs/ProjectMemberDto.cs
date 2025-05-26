using TaskFlow.API.DTOs.AuthDTOs;

namespace TaskFlow.API.DTOs.ProjectDTOs
{
    public class ProjectMemberDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
        public DateTime JoinedAt { get; set; }
        public UserDto User { get; set; }
    }
}
