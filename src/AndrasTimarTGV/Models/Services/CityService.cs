using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;

namespace AndrasTimarTGV.Models.Services
{
    public class CityService : ICityService
    {
        private ICityRepository cityRepository;
        public IEnumerable<City> Cities => cityRepository.Cities;

        public CityService(ICityRepository cityrepo)
        {
            this.cityRepository = cityrepo;
        }
    }
}
