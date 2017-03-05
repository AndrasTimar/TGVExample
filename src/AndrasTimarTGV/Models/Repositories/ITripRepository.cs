using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Repositories
{
    public interface ITripRepository
    {
        IEnumerable<Trip> Trips { get; }

        IEnumerable<Trip> GetTripsByDateAndCities(int fromCityId, int toCityId, DateTime time);
    }
}
