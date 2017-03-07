using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models;
using AndrasTimarTGV.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Controllers
{
    public class HomeController: Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IIntroductionService introductionService;

        public HomeController(IIntroductionService introductionService)
        {
            this.introductionService = introductionService;
        }

        public ViewResult Index(string lang)
        {
            Introduction intro = introductionService.GetIntroductionForLang(lang);
            return View(intro);
        }
    }
}
