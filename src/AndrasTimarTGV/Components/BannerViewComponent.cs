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
        private IBannerTextService bannerTextService;

        public BannerViewComponent(UserManager<AppUser> userManager, IBannerTextService bannerService)
        {
            this.userManager = userManager;
            bannerTextService = bannerService;
        }

        public IViewComponentResult Invoke()
        {
            AppUser user = userManager.FindByNameAsync(HttpContext.User.Identity.Name??"").Result;
            if (user != null)
            {
                return View(bannerTextService.GetBannerTextForLang(user.DefaultLanguage));
            }
            return View(bannerTextService.GetBannerTextForLang(Language.en));
        }
    }
}
