using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AndrasTimarTGV.Models.Repositories
{
    public class EFTripRepository : ITripRepository
    {
        private readonly ApplicationDbContext _context;

        public EFTripRepository(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<Trip> Trips => _context.Trips
            .Include(x=>x.FromCity)
            .Include(x=>x.ToCity);

        public IEnumerable<Trip> GetTripsByDateAndCities(int fromCityId, int toCityId, DateTime time)
        {
            Trip trip = _context.Trips.First();
                      
            return _context.Trips.Include(x => x.FromCity).Include(x => x.ToCity).Where(
                x => x.ToCity.CityId == toCityId
                && x.FromCity.CityId == fromCityId 
                && x.Time.Date == time.Date);
        }

        public Trip GetTipById(int tripId)
        {
            return _context.Trips.Include(x=>x.FromCity).Include(x=>x.ToCity).FirstOrDefault(x => x.TripId == tripId);
        }

        public void UpdateTripSeats(Trip trip)
        {
            Trip entry = _context.Trips.FirstOrDefault(x => x.TripId == trip.TripId);
            if (entry != null)
            {
                entry.FreeBusinessPlaces = trip.FreeBusinessPlaces;
                entry.FreeEconomyPlaces = trip.FreeEconomyPlaces;
                _context.SaveChanges();
            }         
            
        }
    }
}
