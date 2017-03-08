using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AndrasTimarTGV.Models
{
    public class SeedData
    {
        public static void AddSeedData(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            ILogger _logger = app.ApplicationServices.GetRequiredService<ILogger<SeedData>>();
            AddIntroductionSeedDataToDB(context,_logger);
        }
        private static void AddIntroductionSeedDataToDB(ApplicationDbContext context, ILogger _logger) {
            
            if (!context.BannerTexts.Any()) {
                try {
                    context.BannerTexts.AddRange(
                        new BannerText {                           
                            Language = Language.fr,
                            Header= "Bienvenue a la TGV Ticket System",
                            Body = "Réservez vos places ici"
                        },
                        new BannerText {
                            Language = Language.en,
                            Header = "Welcome to the TGV Ticket System",
                            Body = "Book your tickets here"
                        },
                        new BannerText {
                            Language = Language.ne,
                            Header = "Welkom bij de TGV Ticket System",
                            Body = "Reserveer uw tickets hier"
                        }

                    );

                    context.SaveChanges();
                } catch (FileNotFoundException exception) {
                    _logger.LogError("BannerText file was not found : " + exception.FileName + ". BannerText table will be empty for this lang");
                }
            }

            AddCitySeedData(context,_logger);
        }        

        private static void AddCitySeedData(ApplicationDbContext context, ILogger _logger)
        {
            if (!context.Cities.Any()) {
                context.Cities.AddRange(
                    new City{Name="Paris" },
                    new City { Name="Brussels"},
                    new City { Name = "London"},
                    new City { Name = "Amsterdam"},
                    new City { Name="Berlin"}
                    );
                context.SaveChanges();
            }
            AddTripSeedData(context,_logger);
        }

        private static Random random = new Random();
        private static void AddTripSeedData(ApplicationDbContext context, ILogger _logger)
        {
            if (!context.Trips.Any()) {
                foreach (var fromCity in context.Cities)
                {
                    foreach (var toCity in context.Cities)
                    {
                        if (toCity != fromCity)
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                DateTime date = DateTime.Today;

                                for (int j = 0; j < random.Next(1,4); j++) {
                                    date = date.AddHours(random.Next(6, 22));

                                    context.Add(new Trip {
                                        FreeBusinessPlaces = 50,
                                        FromCity = fromCity,
                                        ToCity = toCity,
                                        PricePerPerson = 20+random.Next(10,36),
                                        Time = date.AddDays(i),
                                        FreeEconomyPlaces = 300,
                                    });
                                }
                            }
                                
                        }
                    }
                }
                context.SaveChanges();
            }
        }
    }
}
