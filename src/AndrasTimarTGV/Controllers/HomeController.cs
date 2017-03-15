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
        private readonly IBannerTextService bannerTextService;

        public HomeController(IBannerTextService bannerTextService)
        {            
            this.bannerTextService = bannerTextService;
        }

        public ViewResult Index()
        {           
            return View();
        }
    }
}
