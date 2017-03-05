using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndrasTimarTGV.Models
{
    public interface IIntroductionRepository
    {
        Introduction GetIntroductionsByLang(Language lang);
        IEnumerable<Introduction> Introductions { get; }        
    }
}
