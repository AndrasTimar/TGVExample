using System;
using System.Collections.Generic;
using System.Linq;
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
        private IReservationService reservationService;
        private ITripService tripService;
        private UserManager<AppUser> userManager;
        public ReservationController(IReservationService reservationService, ITripService tripService, UserManager<AppUser> userManager )
        {
            this.userManager = userManager;
            this.reservationService = reservationService;
            this.tripService = tripService;
        }

        public async Task<IActionResult> Reserve(int tripId)
        {
            AppUser user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var trip = tripService.GetTripById(tripId);
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
        public async Task<ActionResult> Proceed(Reservation model) {
            model.User = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            model.Trip = tripService.GetTripById(model.Trip.TripId);
            if (ModelState.IsValid)
            {
                return View(model);
            }
            return View("Reserve",model);
        }

        public async Task<IActionResult> Checkout(Reservation model)
        {
            model.User = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            model.Trip = tripService.GetTripById(model.Trip.TripId);
            if (reservationService.SaveReservation(model))
            {
                return View(model);
            }
            ModelState.AddModelError("", "Not enough free seats");
            return View("Reserve", model);

        }

        public async Task<ViewResult> List()
        {
            AppUser user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            return View(reservationService.GetReservationsByUser(user));
        }

        public IActionResult Delete(int reservationId) {
            Reservation reservation = reservationService.GetReservationsById(reservationId);
            if (reservation != null && reservation.User.UserName == HttpContext.User.Identity.Name) {
                reservationService.Delete(reservation);

            }
            return RedirectToAction("List");
        }
    }       
}
