using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AndrasTimarTGV.Infrastructure
{
    [HtmlTargetElement("td", Attributes = "identity-role")]
    public class RoleUsersTagHelper : TagHelper
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleUsersTagHelper(UserManager<AppUser> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            _userManager = userMgr;
            _roleManager = roleMgr;
        }

        [HtmlAttributeName("identity-role")]
        public string Role { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var names = new List<string>();
            var role = await _roleManager.FindByIdAsync(Role);
            if (role != null)
            {
                foreach (var user in _userManager.Users)
                {
                    if (user != null
                        && await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        names.Add(user.UserName);
                    }
                }
            }
            output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(",", names));
        }        
    }
}
