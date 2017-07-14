using System.Collections.Generic;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Repositories
{
    public interface ICityRepository
    {
        IEnumerable<City> Cities { get; }
    }
}