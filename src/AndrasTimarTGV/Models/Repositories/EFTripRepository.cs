using System;
using System.Collections.Generic;
using System.Linq;
using AndrasTimarTGV.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AndrasTimarTGV.Models.Repositories
{
    public class EfTripRepository : ITripRepository
    {
        private readonly ApplicationDbContext Context;

        public EfTripRepository(ApplicationDbContext ctx)
        {
            Context = ctx;
        }

        public IEnumerable<Trip> Trips => Context.Trips
            .Include(x=>x.FromCity)
            .Include(x=>x.ToCity);

        public IEnumerable<Trip> GetTripsByDateAndCities(int fromCityId, int toCityId, DateTime time)
        {
            return Context.Trips.Include(x => x.FromCity).Include(x => x.ToCity).Where(
                x => x.ToCity.CityId == toCityId
                && x.FromCity.CityId == fromCityId 
                && x.Time.Date == time.Date);
        }

        public Trip GetTipById(int tripId)
        {
            return Context.Trips.Include(x=>x.FromCity).Include(x=>x.ToCity).FirstOrDefault(x => x.TripId == tripId);
        }

        public void UpdateTripSeats(Trip trip)
        {
            Trip entry = Context.Trips.FirstOrDefault(x => x.TripId == trip.TripId);
            if (entry != null)
            {
                entry.FreeBusinessPlaces = trip.FreeBusinessPlaces;
                entry.FreeEconomyPlaces = trip.FreeEconomyPlaces;
                Context.SaveChanges();
            }         
            
        }
    }
}
