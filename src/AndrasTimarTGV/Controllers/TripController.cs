using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;
using AndrasTimarTGV.Models.Services;
using AndrasTimarTGV.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Controller
{
    public class TripController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ITripService _tripService;
        public TripController(ITripService tripService, ICityService cityService)
        {
            this._tripService = tripService;
        }

        [HttpPost]
        public IActionResult List(TripViewModel tripVM)
        {
            if (ModelState.IsValid)
            {
                var resulTrips = _tripService.GetTripsByCitIdsAndDate(tripVM.FromCityId, tripVM.ToCityId, tripVM.Time);
                return View(resulTrips.OrderBy(x=>x.Time.Hour));
            }
            return RedirectToAction("Index","Home");
        }       
    }
}
