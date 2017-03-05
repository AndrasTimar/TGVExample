using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndrasTimarTGV.Models.Services
{   
    public class IntroductionService : IIntroductionService
    {
        private IIntroductionRepository repo;

        public IntroductionService(IIntroductionRepository rep)
        {
            this.repo = rep;
        }

        public Introduction GetIntroductionForLang(string lang)
        {
            return repo.GetIntroductionsByLang((Language) Enum.Parse(typeof(Language), lang));
        }
    }
}
