using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.DTOs.ProjectDTOs
{
    public class CreateProjectDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }
    }
}
