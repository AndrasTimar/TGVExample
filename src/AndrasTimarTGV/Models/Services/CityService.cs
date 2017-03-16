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
        private readonly ICityRepository _cityRepository;
        public IEnumerable<City> Cities => _cityRepository.Cities;

        public CityService(ICityRepository cityrepo)
        {
            _cityRepository = cityrepo;
        }
    }
}
