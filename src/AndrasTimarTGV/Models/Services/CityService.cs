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
        private readonly ICityRepository CityRepository;
        public IEnumerable<City> Cities => CityRepository.Cities;

        public CityService(ICityRepository cityrepo)
        {
            CityRepository = cityrepo;
        }
    }
}