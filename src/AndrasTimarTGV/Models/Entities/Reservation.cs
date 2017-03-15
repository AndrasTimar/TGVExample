using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AndrasTimarTGV.Models.Entities
{
    public enum TravelClass {
        Economy = 0,
        Business = 1
        
    }
    public class Reservation
    {
        public Reservation()
        {
            ReservationTimeStamp = DateTime.Now;
        }
        public int ReservationId { get; set; }
        public AppUser User { get; set; }
        public Trip Trip { get; set; }
        [Range(1,10)]
        public int Seats { get; set; }
        public bool Active { get; set; } = true;        
        public TravelClass TravelClass { get; set; }      
        public DateTime ReservationTimeStamp { get; set; }

    }

    
}
