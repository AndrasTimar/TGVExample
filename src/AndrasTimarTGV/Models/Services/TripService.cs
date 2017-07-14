using System;
using System.Collections.Generic;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;

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
    }
}