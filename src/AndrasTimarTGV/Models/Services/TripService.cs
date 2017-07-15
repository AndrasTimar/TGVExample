using System;
using System.Collections.Generic;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;
using AndrasTimarTGV.Util;

namespace AndrasTimarTGV.Models.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository TripRepository;

        public TripService(ITripRepository tripRepo)
        {
            TripRepository = tripRepo;
        }

        public IEnumerable<Trip> Trips => TripRepository.Trips;

        public IEnumerable<Trip> GetTripsByCitIdsAndDate(int fromCityId, int toCityId, DateTime time)
        {
            return TripRepository.GetTripsByDateAndCities(fromCityId, toCityId, time);
        }

        public Trip GetTripById(int tripId)
        {
            return TripRepository.GetTipById(tripId);
        }

        public void UpdateTripSeats(Trip trip)
        {
            TripRepository.UpdateTripSeats(trip);
        }

        public void DecreaseTripSeatsByReservation(Reservation reservation)
        {
            if (reservation.TravelClass == TravelClass.Business && reservation.Trip.FreeBusinessPlaces >= reservation.Seats) {
                reservation.Trip.FreeBusinessPlaces -= reservation.Seats;
            } else if (reservation.TravelClass == TravelClass.Economy && reservation.Trip.FreeEconomyPlaces >= reservation.Seats) {
                reservation.Trip.FreeEconomyPlaces -= reservation.Seats;
            } else {
                throw new NotEnoughSeatsException("Not enough seats left for your reservation");
            }

            UpdateTripSeats(reservation.Trip);
        }

        public void IncreaseTripSeatsByReservation(Reservation reservation)
        {
            if (reservation.TravelClass == TravelClass.Business && reservation.Trip.FreeBusinessPlaces >= reservation.Seats) {
                reservation.Trip.FreeBusinessPlaces += reservation.Seats;
            } else if (reservation.TravelClass == TravelClass.Economy && reservation.Trip.FreeEconomyPlaces >= reservation.Seats) {
                reservation.Trip.FreeEconomyPlaces += reservation.Seats;
            } else {
                throw new NotEnoughSeatsException("Not enough seats left for your reservation");
            }

            UpdateTripSeats(reservation.Trip);
        }
    }
}