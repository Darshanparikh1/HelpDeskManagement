using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HelpDeskSystem.Models;

namespace HelpDeskSystem.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // Redirect to Razor Pages Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl });
        }

        // Redirect to Razor Pages Register
        [HttpGet]
        public IActionResult Register()
        {
            return RedirectToPage("/Account/Register", new { area = "Identity" });
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
