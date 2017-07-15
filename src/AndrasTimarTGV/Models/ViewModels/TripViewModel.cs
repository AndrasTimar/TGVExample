using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AndrasTimarTGV.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AndrasTimarTGV.Models.ViewModels
{
    public class TripViewModel
    {
        [Required]
        public int FromCityId { get; set; }

        [Required]
        public int ToCityId { get; set; }

        [Required]
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
                selectList.Add(new SelectListItem {Text = city.Name, Selected = false, Value = city.CityId.ToString()});
            }

            return selectList;
        }
    }

    public class DateTimeValidator
    {
        public static ValidationResult ValidateTripTime(DateTime dateTime)
        {
            if (dateTime.Date < DateTime.Now.Date)
            {
                return new ValidationResult("You can not reserve for the past");
            }
            if (dateTime.Date > DateTime.Now.Date.AddDays(14))
            {
                return new ValidationResult("Reservations open 14 days before the trip");
            }
            return ValidationResult.Success;
        }
    }
}