using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.DTOs.ProjectDTOs
{
    public class AddMemberDto
    {
        [Required]
        public int UserId { get; set; }

        public string Role { get; set; } = "Member";
    }
}
