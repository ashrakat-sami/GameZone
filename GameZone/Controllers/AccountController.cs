using GameZone.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Security.Claims;

namespace GameZone.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddAdmin()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAdmin(RegisterUserViewModel newUserVM)
        {


            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.UserName = newUserVM.UserName;
                applicationUser.Email = newUserVM.Email;
                applicationUser.PasswordHash = newUserVM.Password;
                applicationUser.Address = newUserVM.Address;
                IdentityResult result = await _userManager.CreateAsync(applicationUser, newUserVM.Password);
                if (result.Succeeded)
                {
                    //Assign to role
                  await  _userManager.AddToRoleAsync(applicationUser, "Admin");
                    await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }

            }
            return View(newUserVM);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel newUserVM)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.UserName = newUserVM.UserName;
                applicationUser.Email = newUserVM.Email;
                applicationUser.PasswordHash = newUserVM.Password;
                applicationUser.Address = newUserVM.Address;
                IdentityResult result = await _userManager.CreateAsync(applicationUser, newUserVM.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }

            }
            return View(newUserVM);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser =await _userManager.FindByNameAsync(loginVM.UserName);
                if (applicationUser != null) 
                {
                    bool found = await _userManager.CheckPasswordAsync(applicationUser, loginVM.Password);

                    if (found)
                    { 
                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim("Address", applicationUser.Address));
                        await _signInManager.SignInWithClaimsAsync(applicationUser,loginVM.RememberMe,claims);
                        return RedirectToAction("Index", "Home");

                    }
                }

                ModelState.AddModelError("", "User name and password or invalid");

            }

            return View(loginVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
