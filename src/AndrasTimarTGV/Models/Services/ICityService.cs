using System.Collections.Generic;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Services
{
    public interface ICityService
    {
        IEnumerable<City> Cities { get; }
    }
}