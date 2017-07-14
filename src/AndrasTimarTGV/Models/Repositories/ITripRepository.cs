using System;
using System.Collections.Generic;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Repositories
{
    public interface ITripRepository
    {
        IEnumerable<Trip> Trips { get; }
        IEnumerable<Trip> GetTripsByDateAndCities(int fromCityId, int toCityId, DateTime time);
        Trip GetTipById(int tripId);
        void UpdateTripSeats(Trip trip);
    }
}