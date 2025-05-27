using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTOs.TaskDTOs;

namespace TaskFlow.Application.CQRS.Tasks.Commands.CreateTask
{
    public class CreateTaskCommand : IRequest<TaskDto>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } = "ToDo";
        public string Priority { get; set; } = "Medium";
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
        public int CreatedByUserId { get; set; }
        public int? AssignedToUserId { get; set; }
    }
}
