using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndrasTimarTGV.Models
{
    public class EFBannerTextRepository : IBannerTextRepository
    {

        private readonly ApplicationDbContext _context;

        public EFBannerTextRepository(ApplicationDbContext ctx)
        {
            this._context = ctx;
        }

        public IEnumerable<BannerText> BannerTexts => _context.BannerTexts;
        public BannerText GetBannerTextByLang(Language lang)
        {
            return _context.BannerTexts.FirstOrDefault(x => x.Language == lang);
        }
    }
}
