using System.Collections.Generic;
using System.Linq;
using AndrasTimarTGV.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AndrasTimarTGV.Models.Repositories
{
    public class EfReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext Context;

        public EfReservationRepository(ApplicationDbContext ctx)
        {
            Context = ctx;
        }

        public IEnumerable<Reservation> Reservations => Context.Reservations;

        public void SaveReservation(Reservation reservation)
        {
            Context.Reservations.Add(reservation);
            Context.SaveChanges();
        }

        public IEnumerable<Reservation> GetReservationByUserId(string userId)
        {
            return Context.Reservations.Where(x => x.User.Id == userId)
                .Include(x => x.Trip)
                .ThenInclude(x => x.ToCity)
                .Include(x => x.Trip)
                .ThenInclude(x => x.FromCity);
        }

        public Reservation GetReservationById(int reservationId)
        {
            return Context.Reservations.Where(x => x.ReservationId == reservationId)
                .Include(x => x.User)
                .Include(x => x.Trip)
                .ThenInclude(x => x.FromCity)
                .Include(x => x.Trip)
                .ThenInclude(x => x.ToCity).FirstOrDefault();
        }

        public void Delete(Reservation reservation)
        {
            Reservation entry = Context.Reservations.FirstOrDefault(x => x.ReservationId == reservation.ReservationId);
            Context.Remove(entry);
            Context.SaveChanges();
        }
    }
}