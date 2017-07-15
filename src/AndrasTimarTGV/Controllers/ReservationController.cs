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
            try
            {
                ReservationService.ValidateReservationDate(model);
            }
            catch (ReservationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
           
            return ModelState.IsValid ? View(model) : View("Reserve", model);
        }

        [HttpPost]
        public IActionResult Checkout(Reservation model)
        {
            try
            {
                model.User = UserService.FindAppUserByName(HttpContext.User.Identity.Name);
                model.Trip = TripService.GetTripById(model.Trip.TripId);
                ReservationService.SaveReservation(model);
                return View(model);
            }
            catch (ReservationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Reserve", model);
            }
            catch (NullReferenceException ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ViewResult List()
        {
            var user = UserService.FindAppUserByName(HttpContext.User.Identity.Name);
            return View(ReservationService.GetReservationsByUser(user));
        }

        [HttpPost]
        public IActionResult Delete(int reservationId)
        {
            var user = UserService.FindAppUserByName(HttpContext?.User?.Identity?.Name);

            try
            {
                ReservationService.Delete(user, reservationId);
            }
            catch (ReservationOutOfTimeframeException ex)
            {
                TempData["ERROR"] = ex.Message;
            }
            catch (InvalidOperationException)
            {            
                TempData["ERROR"] = "Invalid trip";
            }

            return RedirectToAction("List");
        }
    }
}