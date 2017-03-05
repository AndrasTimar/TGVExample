using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndrasTimarTGV.Models.Entities
{
    public class Trip
    {
        public int TripId { get; set; }
        public City FromCity { get; set; }
        public City ToCity { get; set; }
        public DateTime Time { get; set; }
        public int FreePlaces { get; set; }
        public int TotalPlaces { get; set; }
        public int PricePerPerson { get; set; }
    }
}
