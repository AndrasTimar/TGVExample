using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.DTO;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;
using AndrasTimarTGV.Models.Services;
using AndrasTimarTGV.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Controller
{
    public class TripController : Microsoft.AspNetCore.Mvc.Controller
    {
        private ITripService tripService;
        private ICityRepository ciyRepository;
        public TripController(ITripService tripService, ICityRepository cityRepository)
        {
            this.ciyRepository = cityRepository;
            this.tripService = tripService;
        }

        [HttpPost]
        public ViewResult List(TripViewModel tripVM)
        {
            TripSearchDTO tripSearch = tripVM.TripSearch;            
            var resulTrip = tripService.GetTripsByCitiesAndDate(tripSearch.FromCityId, tripSearch.ToCityId, tripSearch.Time);          
            return View(resulTrip);
        }

        public ViewResult Search()
        {
            return View(new TripViewModel(new TripSearchDTO(),ciyRepository.Cities));
        }
    }
}
