using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;
using Microsoft.AspNetCore.Routing.Constraints;

namespace AndrasTimarTGV.Models.Services
{
    public class TripService : ITripService
    {
        private ITripRepository tripRepository;

        public TripService(ITripRepository tripRepo)
        {
            tripRepository = tripRepo;
        }

        public IEnumerable<Trip> Trips => tripRepository.Trips;
        public IEnumerable<Trip> GetTripsByCitiesAndDate(int fromCityId, int toCityId, DateTime time)
        {
            return tripRepository.GetTripsByDateAndCities(fromCityId, toCityId, time);
        }

        public Trip GetTripById(int tripId)
        {
            return tripRepository.GetTipById(tripId);
        }

        public void UpdateTripSeats(Trip trip)
        {
            tripRepository.UpdateTripSeats(trip);
        }
    }
}
