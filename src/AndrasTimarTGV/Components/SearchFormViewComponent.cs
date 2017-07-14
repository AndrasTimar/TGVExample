using AndrasTimarTGV.Models.Services;
using AndrasTimarTGV.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Components
{
    public class SearchFormViewComponent : ViewComponent
    {
        private readonly ICityService CityService;

        public SearchFormViewComponent(ICityService cityService)
        {
            CityService = cityService;
        }

        public IViewComponentResult Invoke()
        {
            return View(new TripViewModel(CityService.Cities));
        }
    }
}
