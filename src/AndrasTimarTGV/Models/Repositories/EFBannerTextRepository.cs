using System.Collections.Generic;
using System.Linq;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;

namespace AndrasTimarTGV.Models
{
    public class EFBannerTextRepository : IBannerTextRepository
    {
        private readonly ApplicationDbContext Context;

        public EFBannerTextRepository(ApplicationDbContext ctx)
        {
            Context = ctx;
        }

        public IEnumerable<BannerText> BannerTexts => Context.BannerTexts;

        public BannerText GetBannerTextByLang(Language lang)
        {
            return Context.BannerTexts.FirstOrDefault(x => x.Language == lang);
        }
    }
}