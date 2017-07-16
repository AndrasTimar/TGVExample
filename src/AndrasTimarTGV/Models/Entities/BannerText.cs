using System.ComponentModel.DataAnnotations;

namespace AndrasTimarTGV.Models.Entities
{
    public enum Language
    {
        Fr = 0,
        En = 1,
        Ne = 2
    }

    public class BannerText
    {
        public int BannerTextId { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }

        [EnumDataType(typeof(Language))]
        public Language Language { get; set; }
    }
}