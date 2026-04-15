using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using Workcraft.Data;
using Workcraft.Models;
using Workcraft.Models.Enums;
using Workcraft.Services;
using Workcraft.ViewModels;

namespace Workcraft.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly IStatusService _statusService;

        public UserController(UserManager<Users> userManager, ApplicationDbContext context, IStatusService statusService)
        {
            _userManager = userManager;
            _context = context;
            _statusService = statusService;
        }
        
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Dashboard));
        }
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            var userTasks = await _context.TaskItems
            .Where(t => t.AssignedToId == userId)
            .ToListAsync();

            int total = userTasks.Count;
            int completed = userTasks.Count(t => t.Status == WorkTaskStatus.Completed);
            int inProgress = userTasks.Count(t => t.Status == WorkTaskStatus.InProgress);

            double rate = total > 0 ? (double)completed / total * 100 : 0;
            
            ViewBag.TotalTasks = total;
            ViewBag.CompletedTasks = completed;
            ViewBag.InProgressTasks = inProgress;
            ViewBag.CompletionRate = Math.Round(rate);
            ViewBag.CurrentStatus = user?.Status ?? "Available";

            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            ViewBag.Notifications = notifications;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DismissNotification(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true });
        }

        public async Task<IActionResult> TaskDetails()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var tasks = await _context.TaskItems
                .Where(t => t.AssignedToId == userId)
                .OrderByDescending(t => t.DueDate)
                .ToListAsync();

            return View(tasks);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptTask(int taskId)
        {
            var userId = _userManager.GetUserId(User);

            var task = await _context.TaskItems.FindAsync(taskId);

            if(task == null || task.AssignedToId != userId)
                return NotFound();

            task.Status = WorkTaskStatus.InProgress;

            await _context.SaveChangesAsync();

            await _statusService.UpdateEmployeeStatusAsync(userId);

            return RedirectToAction(nameof(TaskDetails));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectTask(int taskId)
        {
            var userId = _userManager.GetUserId(User);

            var task = await _context.TaskItems.FindAsync(taskId);

            if(task == null || task.AssignedToId != userId)
                return NotFound();

            task.Status = WorkTaskStatus.Rejected;

            await _context.SaveChangesAsync();

            await _statusService.UpdateEmployeeStatusAsync(userId);

            return RedirectToAction(nameof(TaskDetails));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeTaskStatus(int taskId, WorkTaskStatus status)
        {
            var userId = _userManager.GetUserId(User);
            var task = await _context.TaskItems.FindAsync(taskId);

            if (task == null || task.AssignedToId != userId)
                return NotFound();

            task.Status = status;
            await _context.SaveChangesAsync();
            await _statusService.UpdateEmployeeStatusAsync(userId);

            return RedirectToAction(nameof(TaskDetails));
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var model = new UserProfileViewModel
            {
                Id = user.Id,
                FullName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Position = user.Position
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Profile");

            var user = await _userManager.FindByIdAsync(model.Id.ToString());

            if (user == null)
                return NotFound();

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            await _userManager.UpdateAsync(user);

            TempData["Success"] = "Profile updated successfully.";

            return RedirectToAction("Profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus([FromBody] StatusViewModel model)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var user = _context.Users.Find(userId);

            if (user != null)
            {
                user.Status = model.Status;
                _context.SaveChanges();
            }

            return Json(new {success = true});
        }
    }
}
