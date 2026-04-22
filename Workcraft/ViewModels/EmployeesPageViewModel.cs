using Workcraft.Models;
using Workcraft.ViewModels;

namespace Workcraft.Models.ViewModels
{
    public class EmployeesPageViewModel
    {
        public List<Users> Employees { get; set; } = new();

        public AddEmployeeViewModel NewEmployee { get; set; } = new();

        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public int BusyEmployees { get; set; }

        public Dictionary<string , EmployeeTaskStats> TaskStats { get; set; } = new();

        public class EmployeeTaskStats
        {
            public int ActiveTasks { get; set; }
            public int CompletedTasks { get; set; }
        }
    }
}