namespace TaskFlow.Domain.Entities
{
    public class ProjectMember
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; } = "Member"; // Owner, Member
        public DateTime JoinedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public Project Project { get; set; }
        public User User { get; set; }
    }
}


