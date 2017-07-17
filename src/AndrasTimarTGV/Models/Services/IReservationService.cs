using System.Collections.Generic;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Services
{
    public interface IReservationService
    {
        Task SaveReservationAsync(Reservation reservation);
        Task<IEnumerable<Reservation>> GetReservationsByUserAsync(AppUser user);
        Task<Reservation> GetReservationsById(int reservationId);
        Task DeleteAsync(AppUser user, int reservationId);
        void ValidateReservationDate(Reservation model);
    }
}