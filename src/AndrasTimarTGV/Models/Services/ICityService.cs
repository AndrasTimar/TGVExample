using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Services
{
    public interface ICityService
    {
        IEnumerable<City> Cities { get; }
    }
}
