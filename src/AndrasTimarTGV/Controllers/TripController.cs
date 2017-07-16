using System.Linq;
using AndrasTimarTGV.Models.Services;
using AndrasTimarTGV.Models.ViewModels;
using AndrasTimarTGV.Util.Filters;
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
        [ModelStateValidityActionFilter]
        public IActionResult List(TripViewModel tripVm)
        {       
            var resulTrips = TripService.GetTripsByCitIdsAndDate(tripVm.FromCityId, tripVm.ToCityId, tripVm.Time);
            return View(resulTrips.OrderBy(x => x.Time.Hour));            
        }
    }
}