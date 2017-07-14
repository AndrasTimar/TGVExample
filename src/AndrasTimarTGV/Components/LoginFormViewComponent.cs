using AndrasTimarTGV.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Components
{
    public class LoginFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(new LoginUserViewModel());
        }
    }
}