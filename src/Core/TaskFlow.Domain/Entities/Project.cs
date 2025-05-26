using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Active"; // Active, Completed, OnHold

        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public User CreatedBy { get; set; }
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public List<ProjectMember> Members { get; set; } = new List<ProjectMember>();
    }
}


