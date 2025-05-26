using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.DTOs.ProjectDTOs
{
    public class UpdateProjectDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Active";
    }
}
