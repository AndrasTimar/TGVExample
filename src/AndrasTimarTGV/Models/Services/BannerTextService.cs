using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndrasTimarTGV.Models.Services
{   
    public class BannerTextService : IBannerTextService
    {
        private IBannerTextRepository repo;

        public BannerTextService(IBannerTextRepository rep)
        {
            this.repo = rep;
        }

        public BannerText GetBannerTextForLang(Language lang)
        {
            return repo.GetBannerTextByLang(lang);
        }
    }
}
