using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Workcraft.Models
{
    public class Users : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        public string? Status { get; set; } = "Available";
        public string? StatusNote { get; set; }


        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public ICollection<TaskItem> Tasks { get; set; }
        public string? Position { get; set; } = "Not Assigned";
    }
}
