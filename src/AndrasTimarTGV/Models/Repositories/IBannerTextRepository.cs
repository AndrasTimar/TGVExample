using System.Collections.Generic;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Repositories
{
    public interface IBannerTextRepository
    {
        BannerText GetBannerTextByLang(Language lang);
        IEnumerable<BannerText> BannerTexts { get; }
    }
}