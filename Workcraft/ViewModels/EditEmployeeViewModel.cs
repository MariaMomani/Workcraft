using System.ComponentModel.DataAnnotations;
namespace Workcraft.ViewModels
{
    public class EditEmployeeViewModel
    {
        public string Id { get; set; }

        [Required]
        public string FullName { get; set; }
        public string Email { get; set; }

        public string? Status { get; set; }

        public bool IsActive { get; set; }
    }
}
