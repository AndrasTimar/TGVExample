using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Services
{
    public interface ITripService
    {
        Task<IEnumerable<Trip>> GetTripsByCitIdsAndDateAsync(int tripSearchFromCityId, int tripSearchToCityId,
            DateTime tripSearchTime);

        Task<Trip> GetTripByIdAsync(int tripId);
        Task UpdateTripSeatsAsync(Trip trip);
        Task DecreaseTripSeatsByReservationAsync(Reservation reservation);
        Task IncreaseTripSeatsByReservationAsync(Reservation reservation);
    }
}