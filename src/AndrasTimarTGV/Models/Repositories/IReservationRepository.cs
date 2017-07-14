using System.Collections.Generic;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Repositories
{
    public interface IReservationRepository
    {
        IEnumerable<Reservation> Reservations { get; }
        void SaveReservation(Reservation reservation);
        IEnumerable<Reservation> GetReservationByUserId(string userId);
        Reservation GetReservationById(int reservationId);
        void Delete(Reservation reservation);
    }
}