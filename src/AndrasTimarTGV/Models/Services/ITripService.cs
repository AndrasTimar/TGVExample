using System;
using System.Collections.Generic;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Services
{
    public interface ITripService
    {
        IEnumerable<Trip> Trips { get; }

        IEnumerable<Trip> GetTripsByCitIdsAndDate(int tripSearchFromCityId, int tripSearchToCityId,
            DateTime tripSearchTime);

        Trip GetTripById(int tripId);
        void UpdateTripSeats(Trip trip);
    }
}