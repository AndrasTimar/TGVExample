using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Components
{
    public class BannerViewComponent : ViewComponent
    {
        private UserManager<AppUser> userManager;
        private IIntroductionService introService;

        public BannerViewComponent(UserManager<AppUser> userManager, IIntroductionService introductionService)
        {
            this.userManager = userManager;
            introService = introductionService;
        }

        public IViewComponentResult Invoke()
        {
            AppUser user = userManager.FindByNameAsync(HttpContext.User.Identity.Name??"").Result;
            if (user != null)
            {
                return View(introService.GetIntroductionForLang(user.DefaultLanguage.ToString()));
            }
            return View(introService.GetIntroductionForLang(Language.en.ToString()));
        }
    }
}
