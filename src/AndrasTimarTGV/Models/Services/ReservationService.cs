using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;

namespace AndrasTimarTGV.Models.Services
{
    public class ReservationService : IReservationService
    {
        private IReservationRepository reservationRepository;
        private ITripService tripService;

        public ReservationService(IReservationRepository reservationRepository, ITripService tripService)
        {
            this.reservationRepository = reservationRepository;
            this.tripService = tripService;
        }

        public IEnumerable<Reservation> Reservations => reservationRepository.Reservations;
        public void SaveReservation(Reservation reservation)
        {            
            reservationRepository.SaveReservation(reservation);
            sendMail(reservation);
        }

        private void sendMail(Reservation reservation)
        {
            //TODO send email
        }

        public IEnumerable<Reservation> GetReservationsByUser(AppUser user)
        {
            return reservationRepository.GetReservationByUserId(user.Id);
        }
    }
}
