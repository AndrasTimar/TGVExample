using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AndrasTimarTGV.Models.ViewModels
{
    public class TripViewModel
    {
        public int FromCityId { get; set; }
        public int ToCityId { get; set; }
        public DateTime Time { get; set; } = DateTime.Today;

        public IEnumerable<SelectListItem> Cities { get; set; }        

        public TripViewModel()
        {
            
        }
        public TripViewModel(IEnumerable<City> cities)
        {
            Cities = ConvertToSelectList(cities);
        }

        private IEnumerable<SelectListItem> ConvertToSelectList(IEnumerable<City> cities)
        {
           var selectList = new List<SelectListItem>();
            foreach (City city in cities)
            {
                 selectList.Add(new SelectListItem {Text=city.Name, Selected = false, Value = city.CityId.ToString()});
            }

            return selectList;
        }
    }
}
