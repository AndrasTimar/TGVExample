using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;
using Microsoft.AspNetCore.Http;
using SendGrid;

namespace AndrasTimarTGV.Models.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ITripService _tripService;

        public ReservationService(IReservationRepository reservationRepository, ITripService tripService)
        {
            this._reservationRepository = reservationRepository;
            this._tripService = tripService;
        }

        public IEnumerable<Reservation> Reservations => _reservationRepository.Reservations;
        public bool SaveReservation(Reservation reservation)
        {            
            Trip trip = _tripService.GetTripById(reservation.Trip.TripId);
            var isFreeEconomy = trip.FreeEconomyPlaces >= reservation.Seats;
            var isFreeBusiness = trip.FreeBusinessPlaces >= reservation.Seats;
            var reserved = false;
            if (reservation.TravelClass == TravelClass.Business && isFreeBusiness)
            {
                trip.FreeBusinessPlaces -= reservation.Seats;
                reserved = true;
            }
            else if(reservation.TravelClass == TravelClass.Economy && isFreeEconomy)
            {
                trip.FreeEconomyPlaces -= reservation.Seats;
                reserved = true;
            }

            if (reserved)
            {
                _reservationRepository.SaveReservation(reservation);
                _tripService.UpdateTripSeats(trip);
                SendMail(reservation);
                return true;
            }
            return false;
        }

        private void SendMail(Reservation reservation)
        {
            var recipient = reservation.User;
            var message = new SendGridMessage
            {
                From = new MailAddress("TGVSchool@gmail.com", "TGV mail service"),
                Subject = "Reservation Confirmation"
            };
            message.AddTo(recipient.Email);
            message.Html = "<p>Your reservation is confirmed! </p>" + reservation.ReservationTimeStamp;
            var apiKey = Environment.GetEnvironmentVariable("SENDMAIL_API_KEY");
            if (apiKey != null)
            {
                var client = new Web(apiKey);

                client.DeliverAsync(message).Wait();
            }
        }

        public IEnumerable<Reservation> GetReservationsByUser(AppUser user)
        {
            return _reservationRepository.GetReservationByUserId(user.Id);
        }

        public Reservation GetReservationsById(int reservationId)
        {
            return _reservationRepository.GetReservationById(reservationId);
        }

        public void Delete(Reservation reservation)
        {
            Trip trip = _tripService.GetTripById(reservation.Trip.TripId);
            _reservationRepository.Delete(reservation);
            if (reservation.TravelClass == TravelClass.Business)
            {
                trip.FreeBusinessPlaces += reservation.Seats;
            }
            else
            {
                trip.FreeEconomyPlaces += reservation.Seats;
            }
                _tripService.UpdateTripSeats(trip);
        }
    }
}
