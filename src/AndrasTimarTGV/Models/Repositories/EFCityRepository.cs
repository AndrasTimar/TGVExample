using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Repositories
{
    public class EFCityRepository : ICityRepository
    {
        private ApplicationDbContext context;
        public EFCityRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<City> Cities => context.Cities;
    }
}
