using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndrasTimarTGV.Models.Services
{
    public interface IIntroductionService
    {
        Introduction GetIntroductionForLang(string lang);
    }
}
