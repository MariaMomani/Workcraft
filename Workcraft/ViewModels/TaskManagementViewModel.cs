using Microsoft.AspNetCore.Mvc.Rendering;
using Workcraft.Models;

namespace Workcraft.ViewModels
{
    public class TaskManagementViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string AssignedToId { get; set; }

        public List<SelectListItem> Employees { get; set; }

        public List<TaskItem> Tasks { get; set; }
    }
}