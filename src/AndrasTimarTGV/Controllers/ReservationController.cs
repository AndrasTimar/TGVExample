using System;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Services;
using AndrasTimarTGV.Util;
using AndrasTimarTGV.Util.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AndrasTimarTGV.Controllers
{
    [Authorize]
    public class ReservationController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IReservationService ReservationService;
        private readonly ITripService TripService;
        private readonly UserManager<AppUser> UserManager;

        public ReservationController(IReservationService reservationService, ITripService tripService,
            UserManager<AppUser> userManager)
        {
            ReservationService = reservationService;
            TripService = tripService;
            UserManager = userManager;
        }

        public async Task<IActionResult> Reserve(int tripId)
        {
            AppUser user = UserManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
            var trip = await TripService.GetTripByIdAsync(tripId);
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
        [ModelStateValidityActionFilter]
        public async Task<ActionResult> Proceed(Reservation model)
        {
            var userTask = UserManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var tripTask = TripService.GetTripByIdAsync(model.Trip.TripId);
            await Task.WhenAll(userTask, tripTask);
            model.User = await userTask;
            model.Trip = await tripTask;

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
        [ModelStateValidityActionFilter]
        public async Task<IActionResult> Checkout(Reservation model)
        {
            try
            {
                var userTask = UserManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var tripTask = TripService.GetTripByIdAsync(model.Trip.TripId);
                await Task.WhenAll(userTask, tripTask);
                model.User = await userTask;
                model.Trip = await tripTask;

                await ReservationService.SaveReservationAsync(model);

                return View(model);
            }
            catch (ReservationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Reserve", model);
            }
        }

        public async Task<ViewResult> List()
        {
            var user = await UserManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var reservations = await ReservationService.GetReservationsByUserAsync(user);
            return View(reservations);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int reservationId)
        {
            var user = await UserManager.FindByNameAsync(HttpContext.User.Identity.Name);

            try
            {
               await ReservationService.DeleteAsync(user, reservationId);
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