using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;

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
    }
}
