using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;
using AndrasTimarTGV.Util;
using Microsoft.Extensions.Logging;
using SendGrid;

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

        public IEnumerable<Reservation> Reservations => ReservationRepository.Reservations;
        
        public void SaveReservation(Reservation reservation)
        {           
            ValidateReservationDate(reservation);
            TripService.DecreaseTripSeatsByReservation(reservation);
            ReservationRepository.SaveReservation(reservation);
            EmailService.SendMail(reservation);
        }

        public IEnumerable<Reservation> GetReservationsByUser(AppUser user)
        {
            return ReservationRepository.GetReservationByUserId(user.Id);
        }

        public Reservation GetReservationsById(int reservationId)
        {
            return ReservationRepository.GetReservationById(reservationId);
        }

        public void Delete(AppUser user, int reservationId)
        {
            Reservation reservation = ReservationRepository.GetReservationById(reservationId);

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

                TripService.IncreaseTripSeatsByReservation(reservation);

                ReservationRepository.Delete(reservation);
            }
            else
            {
                throw new InvalidOperationException("Trip does not exist");
            }
        }

        public void ValidateReservationDate(Reservation reservation)
        {
            if (DateTime.Compare(reservation.Trip.Time, DateTime.Now) <= 0) {
                throw new ReservationOutOfTimeframeException("Reservations can not be made for the past!");
            }
            if (DateTime.Compare(reservation.Trip.Time, DateTime.Now.AddDays(14)) >= 0) {
                throw new ReservationOutOfTimeframeException(
                    "Reservations can be made at most 14 days before departure!");
            }
        }
    }
}