using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;

namespace AndrasTimarTGV.Models.Services
{
    public class BannerTextService : IBannerTextService
    {
        private readonly IBannerTextRepository Repo;

        public BannerTextService(IBannerTextRepository rep)
        {
            Repo = rep;
        }

        public BannerText GetBannerTextForLang(Language lang)
        {
            return Repo.GetBannerTextByLang(lang);
        }
    }
}