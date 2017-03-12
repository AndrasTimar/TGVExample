﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Services
{
   
    public interface IReservationService
    {
        IEnumerable<Reservation> Reservations { get; }
        bool SaveReservation(Reservation reservation);

        IEnumerable<Reservation> GetReservationsByUser(AppUser user);
        Reservation GetReservationsById(int reservationId);
        void Delete(Reservation reservation);
    }
}
