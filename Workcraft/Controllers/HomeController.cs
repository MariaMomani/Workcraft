using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using System.Diagnostics;
using Workcraft.Data;
using Workcraft.Models;

namespace Workcraft.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Admin()
        {
            return RedirectToAction("Index", "Admin");
        }

        [Authorize(Roles ="User")]
        public IActionResult User()
        {
            return RedirectToAction("Index", "User");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(string Name, string Email, string Message)
        {
            var contact = new ContactMessage
            {
                Name = Name,
                Email = Email,
                Message = Message
            };

            _context.ContactMessages.Add(contact);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Message sent successfully!";

            return RedirectToAction("Index");
        }
    }
}