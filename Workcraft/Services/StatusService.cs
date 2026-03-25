using Microsoft.AspNetCore.Identity;
using Workcraft.Data;
using Workcraft.Models;

namespace Workcraft.Services
{
    public class StatusService : IStatusService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Users> _userManager;

        public StatusService(UserManager<Users> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task UpdateEmployeeStatusAsync(string userId)
        {
            var user = await _userManager.FindByNameAsync(userId);
            if (user == null)
                return;

            var hasActiveTask = _context.TaskItems
                .Any(t => t.AssignedToId == userId && 
                          t.Status == Models.Enums.WorkTaskStatus.InProgress);

            user.Status = hasActiveTask ? "Busy" : "Available";

            user.UpdatedAt = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);
        }
    }
}
