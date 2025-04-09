using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tickets = await _context.Tickets
                .Include(t => t.CreatedBy)
                .OrderBy(x => x.CreatedOn)
                .ToListAsync();
            return View(tickets);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous] // Overrides [Authorize] for this action
        public IActionResult Error(int? statusCode = null)
        {
            // Optional: Pass status code to the view for better error handling
            if (statusCode.HasValue)
            {
                ViewData["StatusCode"] = statusCode.Value;
            }
            return View();
        }
    }
}