using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Workcraft.Models;

namespace Workcraft.ViewModels
{
    public class TaskManagementViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        [Required]
        public string AssignedToId { get; set; }


        [ValidateNever]
        public List<SelectListItem>? Employees { get; set; }
        [ValidateNever]
        public List<TaskItem>? Tasks { get; set; }
        [ValidateNever]
        public string SelectedPosition { get; set; }
        [ValidateNever]
        public List<SelectListItem> Positions { get; set; }
    }
}