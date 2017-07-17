using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;
using AndrasTimarTGV.Util;

namespace AndrasTimarTGV.Models.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository TripRepository;

        public TripService(ITripRepository tripRepo)
        {
            TripRepository = tripRepo;
        }
        
        public async Task<IEnumerable<Trip>> GetTripsByCitIdsAndDateAsync(int fromCityId, int toCityId, DateTime time)
        {
            return await TripRepository.GetTripsByDateAndCitiesAsync(fromCityId, toCityId, time);
        }

        public async Task<Trip> GetTripByIdAsync(int tripId)
        {
            return await TripRepository.GetTipByIdAsync(tripId);
        }

        public async Task UpdateTripSeatsAsync(Trip trip)
        {
            await TripRepository.UpdateTripSeatsAsync(trip);
        }

        public async Task DecreaseTripSeatsByReservationAsync(Reservation reservation)
        {
            if (reservation.TravelClass == TravelClass.Business &&
                reservation.Trip.FreeBusinessPlaces >= reservation.Seats)
            {
                reservation.Trip.FreeBusinessPlaces -= reservation.Seats;
            }
            else if (reservation.TravelClass == TravelClass.Economy &&
                     reservation.Trip.FreeEconomyPlaces >= reservation.Seats)
            {
                reservation.Trip.FreeEconomyPlaces -= reservation.Seats;
            }
            else
            {
                throw new NotEnoughSeatsException("Not enough seats left for your reservation");
            }

            await UpdateTripSeatsAsync(reservation.Trip);
        }

        public async Task IncreaseTripSeatsByReservationAsync(Reservation reservation)
        {
            if (reservation.TravelClass == TravelClass.Business &&
                reservation.Trip.FreeBusinessPlaces >= reservation.Seats)
            {
                reservation.Trip.FreeBusinessPlaces += reservation.Seats;
            }
            else if (reservation.TravelClass == TravelClass.Economy &&
                     reservation.Trip.FreeEconomyPlaces >= reservation.Seats)
            {
                reservation.Trip.FreeEconomyPlaces += reservation.Seats;
            }
            else
            {
                throw new NotEnoughSeatsException("Not enough seats left for your reservation");
            }

            await UpdateTripSeatsAsync(reservation.Trip);
        }
    }
}