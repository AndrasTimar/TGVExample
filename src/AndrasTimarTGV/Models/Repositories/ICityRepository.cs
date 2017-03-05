﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Repositories
{
    public interface ICityRepository
    {
        IEnumerable<City> Cities {get;}
    }
}
