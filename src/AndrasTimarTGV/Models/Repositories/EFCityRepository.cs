using System.Collections.Generic;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Repositories
{
    public class EfCityRepository : ICityRepository
    {
        private readonly ApplicationDbContext Context;

        public EfCityRepository(ApplicationDbContext ctx)
        {
            Context = ctx;
        }

        public IEnumerable<City> Cities => Context.Cities;
    }
}