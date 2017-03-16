using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleAdminController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleAdminController(RoleManager<IdentityRole> rolMgr,
            UserManager<AppUser> userMgr)
        {
            _roleManager = rolMgr;
            _userManager = userMgr;
        }

        public ViewResult Index() => View(_roleManager.Roles);

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("","No role found");
            }
            return View("Index", _roleManager.Roles);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error.Description);
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var members = new List<AppUser>();
            var nonMembers = new List<AppUser>();
            foreach (var user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleModificationModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result;
                foreach (var userId in model.IdsToAdd ?? new string[]{})
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.AddToRoleAsync(user,model.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                }
                foreach (var userId in model.IdsToDelete ?? new string[] {})
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }           
            return await Edit(model.RoleId);
        }
    }
}

