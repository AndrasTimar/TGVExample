using System;
using System.Threading.Tasks;
using AndrasTimarTGV.Models;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Controllers
{
    [Authorize]
    public class AccountController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly UserManager<AppUser> UserManager;
        private readonly SignInManager<AppUser> SignInManager;
        private readonly IPasswordHasher<AppUser> PasswordHasher;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IPasswordHasher<AppUser> passHasher)
        {
            PasswordHasher = passHasher;
            UserManager = userManager;
            SignInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await UserManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await SignInManager.SignOutAsync();
                    var result = await SignInManager.PasswordSignInAsync(user, details.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(LoginUserViewModel.Email), "Invalid user or password");
            }
            return View(details);
        }

        [AllowAnonymous]
        public async Task<ViewResult> Register()
        {
            await SignInManager.SignOutAsync();
            return View(new CreateUserViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password == model.PasswordConfirm)
                {
                    var user = new AppUser
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        DefaultLanguage = (Language) Enum.Parse(typeof(Language), model.DefaultLanguage),
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                    };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        TempData["Message"] = "Registration Successful";
                        //TODO: read tempdata in view
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Passwords don't match");
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Edit()
        {
            AppUser user = await UserManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (user != null)
            {
                return View(new CreateUserViewModel()
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    DefaultLanguage = user.DefaultLanguage.ToString(),
                });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password == model.PasswordConfirm)
                {
                    AppUser user = await UserManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.DefaultLanguage = (Language) Enum.Parse(typeof(Language), model.DefaultLanguage);
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PasswordHash = PasswordHasher.HashPassword(user, model.Password);

                    var result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["Message"] = "Changes saved";
                        //TODO: read tempdata in view
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Passwords don't match");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}