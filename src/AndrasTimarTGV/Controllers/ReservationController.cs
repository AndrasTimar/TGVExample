using System;
using System.Linq;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Services;
using AndrasTimarTGV.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Controllers
{
    [Authorize]
    public class ReservationController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IReservationService ReservationService;
        private readonly ITripService TripService;
        private readonly IUserService UserService;

        public ReservationController(IReservationService reservationService, ITripService tripService,
            IUserService userService)
        {
            UserService = userService;
            ReservationService = reservationService;
            TripService = tripService;
        }

        public IActionResult Reserve(int tripId)
        {
            AppUser user = UserService.FindAppUserByName(HttpContext.User.Identity.Name);
            var trip = TripService.GetTripById(tripId);
            if (trip == null)
            {
                return RedirectToAction("Index", "Home");
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
        public ActionResult Proceed(Reservation model)
        {
            model.User = UserService.FindAppUserByName(HttpContext.User.Identity.Name);
            model.Trip = TripService.GetTripById(model.Trip.TripId);
            if (DateTime.Compare(model.Trip.Time, DateTime.Now) < 0)
            {
                ModelState.AddModelError("", "You can not reserve for the past!");
            }
            else if (DateTime.Compare(model.Trip.Time, DateTime.Now.AddDays(14)) > 0)
            {
                ModelState.AddModelError("", "Reservations open 14 days before the trip");
            }
            return ModelState.IsValid ? View(model) : View("Reserve", model);
        }

        public ViewResult Checkout(Reservation model)
        {
            model.User = UserService.FindAppUserByName(HttpContext.User.Identity.Name);
            model.Trip = TripService.GetTripById(model.Trip.TripId);
            if (ReservationService.SaveReservation(model))
            {
                return View(model);
            }
            ModelState.AddModelError("", "Not enough free seats");
            return View("Reserve", model);
        }

        public ViewResult List()
        {
            var user = UserService.FindAppUserByName(HttpContext.User.Identity.Name);
            return View(ReservationService.GetReservationsByUser(user));
        }

        public IActionResult Delete(int reservationId)
        {
            var user = UserService.FindAppUserByName(HttpContext?.User?.Identity?.Name);

            try
            {
                ReservationService.Delete(user, reservationId);
            }
            catch (TooLateReservationException ex)
            {
                TempData["ERROR"] = ex.Message;
            }
            catch (InvalidOperationException ex)
            {            
                TempData["ERROR"] = ex.Message;
            }

            return RedirectToAction("List");
        }
    }
}