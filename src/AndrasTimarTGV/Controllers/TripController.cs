using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;
using AndrasTimarTGV.Models.Services;
using AndrasTimarTGV.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Controller
{
    public class TripController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ITripService tripService;
        private readonly ICityService cityService;
        public TripController(ITripService tripService, ICityService cityService)
        {
            this.cityService = cityService;
            this.tripService = tripService;
        }

        [HttpPost]
        public ViewResult List(TripViewModel tripVM)
        {            
            var resulTrip = tripService.GetTripsByCitiesAndDate(tripVM.FromCityId, tripVM.ToCityId, tripVM.Time);          
            return View(resulTrip);
        }

        public ViewResult Search()
        {
            return View(new TripViewModel(cityService.Cities));
        }
    }
}
