﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AndrasTimarTGV.Models.Repositories
{
    public class EFTripRepository : ITripRepository
    {
        private ApplicationDbContext context;

        public EFTripRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<Trip> Trips => context.Trips
            .Include(x=>x.FromCity)
            .Include(x=>x.ToCity);

        public IEnumerable<Trip> GetTripsByDateAndCities(int fromCityId, int toCityId, DateTime time)
        {
            Trip trip = context.Trips.First();
                      
            return context.Trips.Include(x => x.FromCity).Include(x => x.ToCity).Where(
                x => x.ToCity.CityId == toCityId
                && x.FromCity.CityId == fromCityId 
                && x.Time.Date == time.Date);
        }
       
    }
}