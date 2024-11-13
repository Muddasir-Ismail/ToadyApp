using eTickets.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eTickets.wwwroot.json.ViewModels;
using eTickets.wwwroot.json.Static;

namespace eTickets.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Users()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult Login() => View(new LoginVM());

        [HttpPost]
        public IActionResult Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = _userManager.FindByEmailAsync(loginVM.EmailAddress).Result;
            if (user != null)
            {
                var passwordCheck = _userManager.CheckPasswordAsync(user, loginVM.Password).Result;
                if (passwordCheck)
                {
                    var result = _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false).Result;
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Movies");
                    }
                }
                TempData["Error"] = "Wrong credentials. Please, try again!";
                return View(loginVM);
            }

            TempData["Error"] = "Wrong credentials. Please, try again!";
            return View(loginVM);
        }

        public IActionResult Register() => View(new RegisterVM());

        [HttpPost]
        public IActionResult Register(RegisterVM registerVM, bool isAdmin = false)
        {
            if (!ModelState.IsValid) return View(registerVM);

            // Check if the email already exists
            var user = _userManager.FindByEmailAsync(registerVM.EmailAddress).Result;
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVM);
            }

            // Create a new ApplicationUser
            var newUser = new ApplicationUser
            {
                FullName = registerVM.FullName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress
            };

            // Create the user with the provided password
            var newUserResponse = _userManager.CreateAsync(newUser, registerVM.Password).Result;
            if (newUserResponse.Succeeded)
            {
                // Assign the appropriate role
                if (isAdmin)
                {
                    _userManager.AddToRoleAsync(newUser, UserRoles.Admin).Wait(); // Assign Admin role if isAdmin is true
                }
                else
                {
                    _userManager.AddToRoleAsync(newUser, UserRoles.User).Wait(); // Default role is User
                }

                return View("RegisterCompleted");
            }
            else
            {
                // Handle errors during registration
                foreach (var error in newUserResponse.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerVM);
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync().Wait();
            return RedirectToAction("Index", "Movies");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }
    }
}
