using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;

namespace AndrasTimarTGV.Models.Services
{
    public interface ITripService
    {
        IEnumerable<Trip> Trips { get; }
        IEnumerable<Trip> GetTripsByCitiesAndDate(int tripSearchFromCityId, int tripSearchToCityId, DateTime tripSearchTime);
    }
}
