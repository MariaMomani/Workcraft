namespace Workcraft.Services
{
    public interface IStatusService
    {
        Task UpdateEmployeeStatusAsync(string userId);
    }
}
