namespace Workcraft.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalEmployees { get; set; }
        public int AvailableEmployees { get; set; }
        public int BusyEmployees { get; set; }
        public int TasksInProgress { get; set; }
        public int CompletionRate { get; set; }
        public List<int> WeeklyCompleted { get; set; }
        public List<int> WeeklyInProgress { get; set; }
    }
}
