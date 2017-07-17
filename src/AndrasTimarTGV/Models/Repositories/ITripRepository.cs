using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Repositories
{
    public interface ITripRepository
    {
        Task<List<Trip>> GetTripsByDateAndCitiesAsync(int fromCityId, int toCityId, DateTime time);
        Task<Trip> GetTipByIdAsync(int tripId);
        Task UpdateTripSeatsAsync(Trip trip);
    }
}