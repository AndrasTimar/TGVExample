using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Services;
using AndrasTimarTGV.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Components
{
    public class SearchFormViewComponent : ViewComponent
    {
        private readonly ICityService cityService;

        public SearchFormViewComponent(ICityService cityService)
        {
            this.cityService = cityService;
        }

        public IViewComponentResult Invoke()
        {
            return View(new TripViewModel(cityService.Cities));
        }
    }
}
