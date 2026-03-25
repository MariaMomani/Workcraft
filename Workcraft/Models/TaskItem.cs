using System.ComponentModel.DataAnnotations;
using Workcraft.Models.Enums;

namespace Workcraft.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? DueDate { get; set; }

        public WorkTaskStatus Status { get; set; } = WorkTaskStatus.Pending;

        [Required]
        public string AssignedToId { get; set; }

        public Users AssignedTo { get; set; }
    }
}
