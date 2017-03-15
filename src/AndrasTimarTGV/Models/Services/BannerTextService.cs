using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AndrasTimarTGV.Models.Services
{   
    public class BannerTextService : IBannerTextService
    {
        private readonly IBannerTextRepository _repo;

        public BannerTextService(IBannerTextRepository rep)
        {
            this._repo = rep;
        }

        public BannerText GetBannerTextForLang(Language lang)
        {            
            return _repo.GetBannerTextByLang(lang);
        }
    }
}
