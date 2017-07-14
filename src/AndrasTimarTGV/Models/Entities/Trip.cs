using System;

namespace AndrasTimarTGV.Models.Entities
{
    public class Trip
    {
        public int TripId { get; set; }
        public City FromCity { get; set; }
        public City ToCity { get; set; }
        public DateTime Time { get; set; }
        public int FreeEconomyPlaces { get; set; }
        public int FreeBusinessPlaces { get; set; }
        public int PricePerPerson { get; set; }

        public override string ToString()
        {
            return FromCity.Name + " - " + ToCity.Name + " | " + Time.ToString("D");
        }
    }
}