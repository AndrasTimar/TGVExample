using System.Collections.Generic;
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
        Task DeleteAsync(Reservation reservation);
    }
}