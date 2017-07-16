using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Controllers
{
    public class ErrorController : Microsoft.AspNetCore.Mvc.Controller
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}
