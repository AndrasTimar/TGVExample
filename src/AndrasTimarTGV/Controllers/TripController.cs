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
        public IActionResult List(TripViewModel tripVM)
        {
            if (ModelState.IsValid)
            {
                var resulTrips = TripService.GetTripsByCitIdsAndDate(tripVM.FromCityId, tripVM.ToCityId, tripVM.Time);
                return View(resulTrips.OrderBy(x => x.Time.Hour));
            }
            return RedirectToAction("Index", "Home");
        }
    }
}