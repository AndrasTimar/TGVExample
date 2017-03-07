using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AndrasTimarTGV.Models
{
    public enum Language {
        fr = 0,
        en = 1,
        ne = 2
    }

    public class BannerText
    {
        public int BannerTextId { get;set; }
        public string Header { get; set; }
        public string Body { get; set; }
        [EnumDataType(typeof(Language))]
        public Language Language { get; set; }

    }
}
