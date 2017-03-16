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
        private readonly ITripRepository _tripRepository;

        public TripService(ITripRepository tripRepo)
        {
            _tripRepository = tripRepo;
        }

        public IEnumerable<Trip> Trips => _tripRepository.Trips;
        public IEnumerable<Trip> GetTripsByCitIdsAndDate(int fromCityId, int toCityId, DateTime time)
        {
            return _tripRepository.GetTripsByDateAndCities(fromCityId, toCityId, time);
        }

        public Trip GetTripById(int tripId)
        {
            return _tripRepository.GetTipById(tripId);
        }

        public void UpdateTripSeats(Trip trip)
        {
            _tripRepository.UpdateTripSeats(trip);
        }
    }
}
