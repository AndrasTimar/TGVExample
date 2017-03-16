using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IUserValidator<AppUser> _userValidator;
        private readonly IPasswordValidator<AppUser> _passwordValidator;

        public AdminController(UserManager<AppUser> usrMgr, IPasswordHasher<AppUser> passwordHasher,
            IUserValidator<AppUser> usrValidator, IPasswordValidator<AppUser> pwdValidator)
        {
            _userValidator = usrValidator;
            _passwordValidator = pwdValidator;
            this._passwordHasher = passwordHasher;
            _userManager = usrMgr;
        }

        public ViewResult Index() => View(_userManager.Users);

        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                AddErrorsFromResult(result);
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("Index", _userManager.Users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                return View(user);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                var validEmail = await _userValidator.ValidateAsync(_userManager, user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await _passwordValidator.ValidateAsync(_userManager, user, password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }

                if ((validEmail.Succeeded && validPass == null) ||
                    (validEmail.Succeeded && password != string.Empty && validPass.Succeeded))
                {
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    AddErrorsFromResult(result);
                }
            } else {
                ModelState.AddModelError("", "User Not Found");
            }

            return View(user);
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