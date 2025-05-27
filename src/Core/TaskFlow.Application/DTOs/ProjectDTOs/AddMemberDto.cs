using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Application.DTOs.ProjectDTOs
{
    public class AddMemberDto
    {
        [Required]
        public int UserId { get; set; }

        public string Role { get; set; } = "Member";
    }
}
