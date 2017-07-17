using System.Collections.Generic;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Repositories
{
    public interface IReservationRepository
    {
        Task SaveReservationAsync(Reservation reservation);
        Task<IEnumerable<Reservation>> GetReservationByUserIdAsync(string userId);
        Task<Reservation> GetReservationByIdAsync(int reservationId);
        Task DeleteReservationAsync(Reservation reservation);
    }
}