using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Services;
using AndrasTimarTGV.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Controllers
{
    [Authorize]
    public class ReservationController : Microsoft.AspNetCore.Mvc.Controller {
        private readonly IReservationService _reservationService;
        private readonly ITripService _tripService;
        private readonly IUserService _userService;
        public ReservationController(IReservationService reservationService, ITripService tripService, IUserService userService)
        {
            this._userService = userService;
            this._reservationService = reservationService;
            this._tripService = tripService;
        }

        public IActionResult Reserve(int tripId)
        {
            AppUser user =  _userService.FindAppUserByName(HttpContext.User.Identity.Name);
            var trip = _tripService.GetTripById(tripId);
            if (trip == null)
            {
                return RedirectToAction("Index","Home");
            }
             return View(new Reservation()
            {
                Trip = trip,
                Active = true,  
                User = user,
                Seats = 1
            });
        }

        [HttpPost]
        public  ActionResult Proceed(Reservation model) {
            model.User =  _userService.FindAppUserByName(HttpContext.User.Identity.Name);
            model.Trip = _tripService.GetTripById(model.Trip.TripId);
            if (DateTime.Compare(model.Trip.Time, DateTime.Now) < 0)
            {
                ModelState.AddModelError("","You can not reserve for the past!");
            }else if( DateTime.Compare(model.Trip.Time, DateTime.Now.AddDays(14)) > 0)
            {
                ModelState.AddModelError("","Reservations open 14 days before the trip");
            }
            return ModelState.IsValid ? View(model) : View("Reserve",model);
        }

        public ViewResult Checkout(Reservation model)
        {
            model.User =  _userService.FindAppUserByName(HttpContext.User.Identity.Name);
            model.Trip = _tripService.GetTripById(model.Trip.TripId);
            if (_reservationService.SaveReservation(model))
            {
                return View(model);
            }
            ModelState.AddModelError("", "Not enough free seats");
            return View("Reserve", model);

        }

        public ViewResult List()
        {
            var user =  _userService.FindAppUserByName(HttpContext.User.Identity.Name);
            return View(_reservationService.GetReservationsByUser(user));
        }

        public IActionResult Delete(int reservationId) {         

            var user =  _userService.FindAppUserByName(HttpContext?.User?.Identity?.Name);

            var reservation = user.Reservations.FirstOrDefault(x => x.ReservationId == reservationId);

            if (reservation != null)
            {
                if (DateTime.Compare(reservation.Trip.Time.AddDays(-3), DateTime.Now) < 0)
                {
                    TempData["ERROR"] = "Reservation can not be deleted in the last 3 days!";
                }
                else
                {
                    _reservationService.Delete(reservation);
                }
            }
            return RedirectToAction("List");            
        }
    }       
}
