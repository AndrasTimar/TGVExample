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
        private readonly ILogger<ReservationService> Logger;
        private readonly IReservationRepository ReservationRepository;
        private readonly ITripService TripService;
        private const string EmailTemplateHtmlFile = "email_template.html";
        private const string EnvSengridApiKey = "SENDGRID_API_KEY";

        public ReservationService(IReservationRepository reservationRepository, ITripService tripService,
            ILogger<ReservationService> logger)
        {
            ReservationRepository = reservationRepository;
            TripService = tripService;
            Logger = logger;
        }

        public IEnumerable<Reservation> Reservations => ReservationRepository.Reservations;

        public bool SaveReservation(Reservation reservation)
        {
            Trip trip = TripService.GetTripById(reservation.Trip.TripId);
            var isFreeEconomy = trip.FreeEconomyPlaces >= reservation.Seats;
            var isFreeBusiness = trip.FreeBusinessPlaces >= reservation.Seats;
            var reserved = false;
            if (reservation.TravelClass == TravelClass.Business && isFreeBusiness)
            {
                trip.FreeBusinessPlaces -= reservation.Seats;
                reserved = true;
            }
            else if (reservation.TravelClass == TravelClass.Economy && isFreeEconomy)
            {
                trip.FreeEconomyPlaces -= reservation.Seats;
                reserved = true;
            }

            if (reserved)
            {
                ReservationRepository.SaveReservation(reservation);
                TripService.UpdateTripSeats(trip);
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
            try
            {
                message.Html = File.ReadAllText(EmailTemplateHtmlFile)
                    .Replace("{{NAME}}", recipient.FirstName + " " + recipient.LastName)
                    .Replace("{{TRIP_SUMMARY}}", reservation.Trip.ToString());

                var apiKey = Environment.GetEnvironmentVariable(EnvSengridApiKey);
                if (apiKey != null)
                {
                    var client = new Web(apiKey);

                    client.DeliverAsync(message).Wait();
                }
                else
                {
                    Logger.LogError("Confirmation email was not sent, sendgrid api key not found");
                }
            }
            catch (FileNotFoundException e)
            {
                Logger.LogError("Confirmation email was not sent, template missing: " + e.Message);
            }
        }

        public IEnumerable<Reservation> GetReservationsByUser(AppUser user)
        {
            return ReservationRepository.GetReservationByUserId(user.Id);
        }

        public Reservation GetReservationsById(int reservationId)
        {
            return ReservationRepository.GetReservationById(reservationId);
        }

        private void Delete(Reservation reservation, Trip trip)
        {
            ReservationRepository.Delete(reservation);
            if (reservation.TravelClass == TravelClass.Business)
            {
                trip.FreeBusinessPlaces += reservation.Seats;
            }
            else
            {
                trip.FreeEconomyPlaces += reservation.Seats;
            }
        }

        public void Delete(AppUser user, int reservationId)
        {
            Reservation reservation = ReservationRepository.GetReservationById(reservationId);
          
            if (reservation != null)
            {
                if (reservation.User != user) {
                    throw new InvalidOperationException("Trip doesnt belong to logged in user");
                }
                if (DateTime.Compare(reservation.Trip.Time.AddDays(-3), DateTime.Now) < 0)
                {
                    throw new TooLateReservationException("Reservations can not be deleted in the last 3 days!");
                }
                

                Delete(reservation, reservation.Trip);

                TripService.UpdateTripSeats(reservation.Trip);
            }
            throw new InvalidOperationException("Trip does not exist");
        }
    }
}