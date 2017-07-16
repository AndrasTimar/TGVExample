using System.Collections.Generic;
using System.Linq;
using AndrasTimarTGV.Models.Entities;

namespace AndrasTimarTGV.Models.Repositories
{
    public class EfBannerTextRepository : IBannerTextRepository
    {
        private readonly ApplicationDbContext Context;

        public EfBannerTextRepository(ApplicationDbContext ctx)
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