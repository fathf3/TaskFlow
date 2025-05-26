using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "ToDo"; // ToDo, InProgress, Testing, Done

        [MaxLength(50)]
        public string Priority { get; set; } = "Medium"; // Low, Medium, High, Critical

        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }

        // Foreign Keys
        public int ProjectId { get; set; }
        public int CreatedByUserId { get; set; }
        public int? AssignedToUserId { get; set; }

        // Navigation Properties
        public Project Project { get; set; }
        public User CreatedBy { get; set; }
        public User? AssignedTo { get; set; }
    }
}


