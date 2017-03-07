using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndrasTimarTGV.Models
{
    public interface IBannerTextRepository
    {
        BannerText GetBannerTextByLang(Language lang);
        IEnumerable<BannerText> BannerTexts { get; }        
    }
}
