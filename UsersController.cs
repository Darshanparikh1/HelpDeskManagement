using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HelpDeskSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context , UserManager<ApplicationUser> userManager , 
             RoleManager<IdentityRole> roleManager , SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        // GET: UsersController
        public async Task<ActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // GET: UsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ApplicationUser user)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                ApplicationUser registeredUser = new();
                registeredUser.FirstName = user.FirstName;
                registeredUser.UserName = user.UserName;
                registeredUser.MiddleName = user.MiddleName;
                registeredUser.LastName = user.LastName;
                registeredUser.NormalizedUserName = user.UserName;
                registeredUser.Email = user.Email;
                registeredUser.EmailConfirmed = true;
                registeredUser.Gender = user.Gender;
                registeredUser.City = user.City;
                registeredUser.Country = user.Country;
                registeredUser.PhoneNumber = user.PhoneNumber;

                // Ensure a valid password is provided
                if (string.IsNullOrWhiteSpace(user.PasswordHash))
                {
                    ModelState.AddModelError("Password", "Password is required.");
                    return View(user);
                }

                var result = await _userManager.CreateAsync(registeredUser, user.PasswordHash);
                if (result.Succeeded)
                {
                    var activity = new AuditTrail
                    {
                        Action = "Create",
                        TimeStamp = DateTime.Now,
                        IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        UserId = userId,
                        Module = "Users",
                        AffectedTable = "Users"
                    };

                    _context.Add(activity);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "User Details successfully Created!";

                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the user.");
                return View(user);
            }
        }


        // GET: UsersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
