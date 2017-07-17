using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AndrasTimarTGV.Models.Repositories
{
    public class EfReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext Context;
        private readonly ITripRepository TripRepository;

        public EfReservationRepository(ApplicationDbContext ctx, ITripRepository tripRepository)
        {
            Context = ctx;
            TripRepository = tripRepository;
        }
        

        public async Task SaveReservationAsync(Reservation reservation)
        {
            await Context.Reservations.AddAsync(reservation);
            await Context.SaveChangesAsync();
            await TripRepository.UpdateTripSeatsAsync(reservation.Trip);
        }

        public async Task<IEnumerable<Reservation>> GetReservationByUserIdAsync(string userId)
        {
            return await Context.Reservations.Where(x => x.User.Id == userId)
                .Include(x => x.Trip)
                .ThenInclude(x => x.ToCity)
                .Include(x => x.Trip)
                .ThenInclude(x => x.FromCity).ToListAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(int reservationId)
        {
            return await Context.Reservations.Where(x => x.ReservationId == reservationId)
                .Include(x => x.User)
                .Include(x => x.Trip)
                .ThenInclude(x => x.FromCity)
                .Include(x => x.Trip)
                .ThenInclude(x => x.ToCity).FirstOrDefaultAsync();
        }

        public async Task DeleteReservationAsync(Reservation reservation)
        {
            Reservation entry = await Context.Reservations.FirstOrDefaultAsync(x => x.ReservationId == reservation.ReservationId);
            Context.Remove(entry);
            await Context.SaveChangesAsync();
        }
    }
}