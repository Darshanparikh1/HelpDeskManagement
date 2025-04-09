using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var model = new DashboardViewModel
        {
            TotalTickets = _context.Tickets.Count(),
            SolvedTickets = _context.Tickets.Count(t => t.Status == "Solved"),
            PendingTickets = _context.Tickets.Count(t => t.Status == "Pending")
        };

        return View(model);
    }
}
