using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
