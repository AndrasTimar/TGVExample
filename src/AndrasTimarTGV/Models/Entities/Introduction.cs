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

    public class Introduction
    {
        public int IntroductionId { get; set; }
        public string Name { get; set; }

        public string Content { get; set; }

        [EnumDataType(typeof(Language))]
        public Language Language { get; set; }

    }
}
