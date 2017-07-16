using System.Linq;
using AndrasTimarTGV.Models.Services;
using AndrasTimarTGV.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Controller
{
    public class TripController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ITripService TripService;

        public TripController(ITripService tripService, ICityService cityService)
        {
            TripService = tripService;
        }

        [HttpPost]
        public IActionResult List(TripViewModel tripVm)
        {
            if (ModelState.IsValid)
            {
                var resulTrips = TripService.GetTripsByCitIdsAndDate(tripVm.FromCityId, tripVm.ToCityId, tripVm.Time);
                return View(resulTrips.OrderBy(x => x.Time.Hour));
            }
            return RedirectToAction("Index", "Home");
        }
    }
}