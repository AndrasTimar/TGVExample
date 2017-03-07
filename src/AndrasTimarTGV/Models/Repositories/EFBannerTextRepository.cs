using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndrasTimarTGV.Models
{
    public class EFBannerTextRepository : IBannerTextRepository
    {

        private ApplicationDbContext context;

        public EFBannerTextRepository(ApplicationDbContext ctx)
        {
            this.context = ctx;
        }

        public IEnumerable<BannerText> BannerTexts => context.BannerTexts;
        public BannerText GetBannerTextByLang(Language lang)
        {
            return context.BannerTexts.FirstOrDefault(x => x.Language == lang);
        }
    }
}
