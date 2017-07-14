using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Services
{
    public interface IBannerTextService
    {
        BannerText GetBannerTextForLang(Language lang);
    }
}