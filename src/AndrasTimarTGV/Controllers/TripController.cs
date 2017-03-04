using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Controller
{
    public class TripController : Microsoft.AspNetCore.Mvc.Controller
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}
