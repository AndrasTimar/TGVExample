using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AndrasTimarTGV.Models.Repositories
{
    public class EFReservationRepository : IReservationRepository
    {
        private ApplicationDbContext context;

        public EFReservationRepository(ApplicationDbContext ctx) {
            context = ctx;
        }
        public IEnumerable<Reservation>  Reservations => context.Reservations;
        public void SaveReservation(Reservation reservation)
        {
            context.Reservations.Add(reservation);
            context.SaveChanges();
        }

        public IEnumerable<Reservation> GetReservationByUserId(string userId)
        {
            return
                context.Reservations.Where(x => x.User.Id == userId)
                    .Include(x => x.Trip)
                    .ThenInclude(x => x.ToCity)
                    .Include(x => x.Trip)
                    .ThenInclude(x => x.FromCity);
        }

        public Reservation GetReservationById(int reservationId)
        {
            return context.Reservations.Where(x => x.ReservationId == reservationId)
                .Include(x => x.User)
                .Include(x => x.Trip)
                .ThenInclude(x => x.FromCity)
                .Include(x => x.Trip)
                .ThenInclude(x => x.ToCity).FirstOrDefault();
        }

        public void Delete(Reservation reservation)
        {
            Reservation entry = context.Reservations.FirstOrDefault(x => x.ReservationId == reservation.ReservationId);
            context.Remove(entry);
            context.SaveChanges();
        }
    }
}
