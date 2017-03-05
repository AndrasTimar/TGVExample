using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndrasTimarTGV.Models.DTO
{
    public class TripSearchDTO
    {
        public int FromCityId { get; set; }
        public int ToCityId { get; set; }
        public DateTime Time { get; set; }
    }
}
