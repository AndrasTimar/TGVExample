using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Controllers
{
    public class HomeController: Microsoft.AspNetCore.Mvc.Controller
    {
        private IIntroductionRepository repo;

        public HomeController(IIntroductionRepository repository)
        {
            repo = repository;
        }
        public ViewResult Index(string lang)
        {
            ViewBag.content = repo.getIntroductionsByLangString(lang).Content;
            return View();
        }
    }
}
