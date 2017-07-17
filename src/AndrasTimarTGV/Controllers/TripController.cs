using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Services;
using AndrasTimarTGV.Models.ViewModels;
using AndrasTimarTGV.Util.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Controllers
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
        public async Task<IActionResult> List(TripViewModel tripVm)
        {
            var resulTrips = await TripService.GetTripsByCitIdsAndDateAsync(tripVm.FromCityId, tripVm.ToCityId, tripVm.Time);
            return View(resulTrips.OrderBy(x => x.Time.Hour));
        }
    }
}