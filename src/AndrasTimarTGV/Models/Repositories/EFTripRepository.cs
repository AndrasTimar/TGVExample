using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<List<Trip>> GetTripsByDateAndCitiesAsync(int fromCityId, int toCityId, DateTime time)
        {
            return await Context.Trips.Include(x => x.FromCity).Include(x => x.ToCity).Where(
                x => x.ToCity.CityId == toCityId
                     && x.FromCity.CityId == fromCityId
                     && x.Time.Date == time.Date).ToListAsync();
        }

        public async Task<Trip> GetTipByIdAsync(int tripId)
        {
            return await Context.Trips.Include(x => x.FromCity).Include(x => x.ToCity)
                .FirstOrDefaultAsync(x => x.TripId == tripId);
        }

        public async Task UpdateTripSeatsAsync(Trip trip)
        {
            Trip entry = await Context.Trips.FirstOrDefaultAsync(x => x.TripId == trip.TripId);
            if (entry != null)
            {
                entry.FreeBusinessPlaces = trip.FreeBusinessPlaces;
                entry.FreeEconomyPlaces = trip.FreeEconomyPlaces;
                await Context.SaveChangesAsync();
            }
        }
    }
}