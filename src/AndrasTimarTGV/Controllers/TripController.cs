﻿using System;
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
        private readonly ITripService tripService;
        private readonly ICityService cityService;
        public TripController(ITripService tripService, ICityService cityService)
        {
            this.cityService = cityService;
            this.tripService = tripService;
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ViewResult List(TripViewModel tripVM)
        {
            //TODO: Modelstate.isvalid ??
            if (ModelState.IsValid)
            {
                var resulTrip = tripService.GetTripsByCitiesAndDate(tripVM.FromCityId, tripVM.ToCityId, tripVM.Time);
                return View(resulTrip);
            }
            return View("Search");
        }

        public ViewResult Search()
        {
            return View(new TripViewModel(cityService.Cities));
        }
    }
}
