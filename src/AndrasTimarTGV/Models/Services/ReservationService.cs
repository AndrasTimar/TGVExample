using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;

namespace AndrasTimarTGV.Models.Services
{
    public class ReservationService : IReservationService
    {
        private IReservationRepository reservationRepository;
        private ITripService tripService;

        public ReservationService(IReservationRepository reservationRepository, ITripService tripService)
        {
            this.reservationRepository = reservationRepository;
            this.tripService = tripService;
        }

        public IEnumerable<Reservation> Reservations => reservationRepository.Reservations;
        public bool SaveReservation(Reservation reservation)
        {            
            Trip trip = tripService.GetTripById(reservation.Trip.TripId);
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
                reservationRepository.SaveReservation(reservation);
                tripService.UpdateTripSeats(trip);
                sendMail(reservation);
                return true;
            }
            return false;
        }

        private void sendMail(Reservation reservation)
        {
            //TODO send email
        }

        public IEnumerable<Reservation> GetReservationsByUser(AppUser user)
        {
            return reservationRepository.GetReservationByUserId(user.Id);
        }

        public Reservation GetReservationsById(int reservationId)
        {
            return reservationRepository.GetReservationById(reservationId);
        }

        public void Delete(Reservation reservation)
        {
            Trip trip = tripService.GetTripById(reservation.Trip.TripId);
            reservationRepository.Delete(reservation);
            if (reservation.TravelClass == TravelClass.Business)
            {
                trip.FreeBusinessPlaces += reservation.Seats;
            }
            else
            {
                trip.FreeEconomyPlaces += reservation.Seats;
            }
                tripService.UpdateTripSeats(trip);
        }
    }
}
