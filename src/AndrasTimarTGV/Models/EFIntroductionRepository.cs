using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndrasTimarTGV.Models
{
    public class EFIntroductionRepository : IIntroductionRepository
    {

        private ApplicationDbContext context;

        public EFIntroductionRepository(ApplicationDbContext ctx)
        {
            this.context = ctx;
        }

        public IEnumerable<Introduction> Introductions => context.Introductions;
        public Introduction getIntroductionsByLangString(string lang)
        {
            return context.Introductions.FirstOrDefault(x => x.Language == (Language) Enum.Parse(typeof(Language),lang));
        }
    }
}
