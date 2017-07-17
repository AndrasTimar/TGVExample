using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;
using AndrasTimarTGV.Util;

namespace AndrasTimarTGV.Models.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository ReservationRepository;
        private readonly ITripService TripService;
        private readonly IEmailService EmailService;

        public ReservationService(IReservationRepository reservationRepository, ITripService tripService,
            IEmailService emailService)
        {
            ReservationRepository = reservationRepository;
            TripService = tripService;
            EmailService = emailService;
        }

        public async Task SaveReservationAsync(Reservation reservation)
        {
            ValidateReservationDate(reservation);
            await TripService.DecreaseTripSeatsByReservationAsync(reservation);
            await ReservationRepository.SaveReservationAsync(reservation);
            EmailService.SendMail(reservation);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByUserAsync(AppUser user)
        {
            return await ReservationRepository.GetReservationByUserIdAsync(user.Id);
        }

        public async Task<Reservation> GetReservationsById(int reservationId)
        {
            return await ReservationRepository.GetReservationByIdAsync(reservationId);
        }

        public async Task DeleteAsync(AppUser user, int reservationId)
        {
            Reservation reservation = await ReservationRepository.GetReservationByIdAsync(reservationId);

            if (reservation != null)
            {
                if (reservation.User != user)
                {
                    throw new InvalidOperationException("Trip does not belong to logged in user");
                }

                var reservationTrip = reservation.Trip;

                if (DateTime.Compare(reservationTrip.Time.AddDays(-3), DateTime.Now) <= 0)
                {
                    throw new ReservationOutOfTimeframeException("Reservations can not be deleted in the last 3 days!");
                }

                var increaseSeatsTask = TripService.IncreaseTripSeatsByReservationAsync(reservation);

                var deleteReservationTask = ReservationRepository.DeleteReservationAsync(reservation);

                await Task.WhenAll(increaseSeatsTask, deleteReservationTask);
            }
            else
            {
                throw new InvalidOperationException("Trip does not exist");
            }
        }

        public void ValidateReservationDate(Reservation reservation)
        {
            if (DateTime.Compare(reservation.Trip.Time, DateTime.Now) <= 0)
            {
                throw new ReservationOutOfTimeframeException("Reservations can not be made for the past!");
            }
            if (DateTime.Compare(reservation.Trip.Time, DateTime.Now.AddDays(14)) >= 0)
            {
                throw new ReservationOutOfTimeframeException(
                    "Reservations can be made at most 14 days before departure!");
            }
        }
    }
}