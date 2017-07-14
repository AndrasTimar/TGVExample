using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Controllers
{
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}