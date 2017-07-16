using AndrasTimarTGV.Models;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Components
{
    public class BannerViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> UserManager;
        private readonly IBannerTextService BannerTextService;

        public BannerViewComponent(UserManager<AppUser> userManager, IBannerTextService bannerService)
        {
            UserManager = userManager;
            BannerTextService = bannerService;
        }

        public IViewComponentResult Invoke()
        {
            AppUser user = UserManager.FindByNameAsync(HttpContext.User.Identity.Name ?? "").Result;
            if (user != null)
            {
                return View(BannerTextService.GetBannerTextForLang(user.DefaultLanguage));
            }
            return View(BannerTextService.GetBannerTextForLang(Language.En));
        }
    }
}