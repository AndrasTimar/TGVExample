using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Repositories
{
    public class CityRepository : ICityRepository
    {
        private ApplicationDbContext context;
        public CityRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<City> Cities => context.Cities;
    }
}
