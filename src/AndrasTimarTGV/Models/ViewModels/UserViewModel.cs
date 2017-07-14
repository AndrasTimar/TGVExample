using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AndrasTimarTGV.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AndrasTimarTGV.Models.ViewModels
{
    public class CreateUserViewModel
    {
        public IEnumerable<SelectListItem> Languages => new List<SelectListItem>()
        {
            new SelectListItem()
            {
                Text = "French",
                Value = Language.Fr.ToString(),
                Selected = false
            },
            new SelectListItem()
            {
                Text = "English",
                Value = Language.En.ToString(),
                Selected = false
            },
            new SelectListItem()
            {
                Text = "Dutch",
                Value = Language.Ne.ToString(),
                Selected = false
            },
        };

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [UIHint("password")]
        [Required]
        public string Password { get; set; }

        [UIHint("password")]
        [Required]
        public string PasswordConfirm { get; set; }

        public string DefaultLanguage { get; set; } = "en";

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }

    public class LoginUserViewModel
    {
        [Required]
        [UIHint("email")]
        public string Email { get; set; }

        [Required]
        [UIHint("password")]
        public string Password { get; set; }
    }

    public class RoleEditModel
    {
        public IdentityRole Role { get; set; }

        public IEnumerable<AppUser> Members { get; set; }

        public IEnumerable<AppUser> NonMembers { get; set; }
    }

    public class RoleModificationModel
    {
        [Required]

        public string RoleName { get; set; }

        public string RoleId { get; set; }

        public string[] IdsToAdd { get; set; }

        public string[] IdsToDelete { get; set; }
    }
}