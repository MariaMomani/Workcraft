using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Workcraft.Data;
using Workcraft.Models;
using Workcraft.Models.Enums;
using Workcraft.Models.ViewModels;
using Workcraft.Services;
using Workcraft.ViewModels;

namespace Workcraft.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IStatusService _statusService;
        private readonly UserManager<Users> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<Users> userManager, ApplicationDbContext context, IStatusService statusService)
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
            var usersInRole = await _userManager.GetUsersInRoleAsync("User");

            var totalEmployees = usersInRole.Count;

            var availableEmployees = usersInRole.Count(u => u.Status == "Available");

            var busyEmployees = usersInRole.Count(u => u.Status == "Busy");

            var tasksInProgress = _context.TaskItems
                .Count(t => t.Status == WorkTaskStatus.InProgress);

            var model = new DashboardViewModel
            {
                TotalEmployees = totalEmployees,
                AvailableEmployees = availableEmployees,
                BusyEmployees = busyEmployees,
                TasksInProgress = tasksInProgress
            };

            return View(model);
        }

        public async Task<IActionResult> Employees()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("User");

            var model = new EmployeesPageViewModel
            {
                Employees = usersInRole.ToList(),
                NewEmployee = new AddEmployeeViewModel(),
                TotalEmployees = usersInRole.Count,
                ActiveEmployees = usersInRole.Count(u => u.IsActive),
                BusyEmployees = usersInRole.Count(u => u.Status == "Busy")
            };

            return View(model);
        }

        public async Task<IActionResult> EmployeeDetails(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee(EmployeesPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Employees = _userManager.Users.ToList();
                return View("Employees", model);
            }

            var user = new Users
            {
                UserName = model.NewEmployee.Email,
                Email = model.NewEmployee.Email,
                FullName = model.NewEmployee.FullName,
                Status = "Available",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.NewEmployee.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                TempData["Success"] = "Agent created successfully!";
                return RedirectToAction("Employees");
            }

            model.Employees = _userManager.Users.ToList();

            foreach (var error in result.Errors)
            {
                TempData["Error"] = error.Description;
            }

            return View("Employees", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction(nameof(Employees));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee(Users model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return NotFound();

            user.FullName = model.FullName;
            user.Status = model.Status;

            if (user.Email != model.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to update email.");
                    return RedirectToAction("Employees");
                }

                var setUserNameResult = await _userManager.SetUserNameAsync(user, model.Email);
                if (!setUserNameResult.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to update username.");
                    return RedirectToAction("Employees");
                }
            }

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Employees");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                user.IsActive = !user.IsActive;
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction(nameof(Employees));
        }

        public async Task<IActionResult> Tasks()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("User");

            var model = new TaskManagementViewModel
            {
                Employees = usersInRole
                    .Where(u => u.IsActive)
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = u.FullName
                    }).ToList(),

                Tasks = _context.TaskItems
                    .Include(t => t.AssignedTo)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask(TaskManagementViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var users = await _userManager.GetUsersInRoleAsync("User");
                model.Employees = users.Select(u => new SelectListItem { Value = u.Id, Text = u.FullName }).ToList();
                model.Tasks = _context.TaskItems.Include(t => t.AssignedTo).ToList();
                return View("Tasks", model);
            }

            var task = new TaskItem
            {
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate,
                AssignedToId = model.AssignedToId,
                Status = WorkTaskStatus.Pending,
                CreatedAt = DateTime.Now
            };

            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();

            _context.Notifications.Add(new Notification
            {
                UserId = task.AssignedToId,
                Message = $"New task assigned: {task.Title}",
            });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Tasks));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeTaskStatus(int taskId, WorkTaskStatus status)
        {
            var task = await _context.TaskItems.FindAsync(taskId);

            if (task == null) 
                return NotFound();

            task.Status = status;

            await _context.SaveChangesAsync();

            await _statusService.UpdateEmployeeStatusAsync(task.AssignedToId);

            return RedirectToAction(nameof(Tasks));
        }


        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound("User not found");

            return View(user);
        }

        [Authorize]
        public async Task<IActionResult> CompanyInfo()
        {
            var settings = await _context.CompanySettings.FirstOrDefaultAsync();

            if (settings == null)
            {
                return View(new CompanyInfoViewModel());
            }

            var model = new CompanyInfoViewModel
            {
                CompanyName = settings.CompanyName,
                TaxId = settings.TaxId,
                Website = settings.Website,
                LegalName = settings.LegalName,
                ContactEmail = settings.ContactEmail,
                Address = settings.Address
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCompany(CompanyInfoViewModel model)
        {
            if (!ModelState.IsValid) return View("CompanyInfo", model);

            var settings = await _context.CompanySettings.FirstOrDefaultAsync();

            if (settings == null)
            {
                settings = new CompanySettings();
                _context.Add(settings);
            }

            settings.CompanyName = model.CompanyName;
            settings.Industry = model.Industry;
            settings.CompanySize = model.CompanySize; 
            settings.Website = model.Website;
            settings.LegalName = model.LegalName;
            settings.TaxId = model.TaxId;
            settings.ContactEmail = model.ContactEmail;
            settings.Address = model.Address;
            settings.IsVerified = model.IsVerified;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Company details updated successfully!";
            return RedirectToAction(nameof(CompanyInfo));
        }

        public async Task<IActionResult> Messages()
        {
            var messages = await _context.ContactMessages
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

            return View(messages);
        }
    }
}