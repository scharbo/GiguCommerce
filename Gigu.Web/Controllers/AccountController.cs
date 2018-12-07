using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gigu.Web.Models;
using Gigu.Web.ViewModels;
using Gigu.Web.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gigu.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<Customer> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Customer> _signInManager;

        public AccountController(ILogger<AccountController> logger,
            UserManager<Customer> userManager, 
            RoleManager<IdentityRole> roleManager, 
            SignInManager<Customer> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer { UserName = registerVM.Email, Email = registerVM.Email };
                var result = await _userManager.CreateAsync(customer, registerVM.Password);

                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("SiteUser").Result)
                    {
                        var role = new IdentityRole();
                        role.Name = "SiteUser";

                        IdentityResult roleResult = _roleManager.CreateAsync(role).Result;

                        if (!roleResult.Succeeded)
                        {
                            ModelState.AddModelError("", "Somethings went wrong !");
                            return View(registerVM);
                        }
                    }

                    await _userManager.AddToRoleAsync(customer, "SiteUser");
                    await _signInManager.SignInAsync(customer, isPersistent: false);
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(registerVM);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    loginVM.Email,
                    loginVM.Password,
                    loginVM.RememberMe,
                    false);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Failed to login");
            return View(loginVM);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }        
    }
}