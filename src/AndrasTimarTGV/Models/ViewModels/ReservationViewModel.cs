using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.ViewModels
{
    public class ReservationViewModel
    {
        public int Lofasz { get; set; }
        public Trip Trip { get; set; }
        public int Seats { get; set; }
        public TravelClass TravelClass { get; set; }

        public AppUser User { get; set; }

    }
}
