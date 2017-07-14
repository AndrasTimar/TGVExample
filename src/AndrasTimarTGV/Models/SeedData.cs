using System;
using System.IO;
using System.Linq;
using AndrasTimarTGV.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AndrasTimarTGV.Models
{
    public class SeedData
    {
        public static void AddSeedData(IApplicationBuilder app) {
            ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            ILogger logger = app.ApplicationServices.GetRequiredService<ILogger<SeedData>>();
            AddIntroductionSeedDataToDb(context, logger);
        }

        private static void AddIntroductionSeedDataToDb(ApplicationDbContext context, ILogger logger)
        {
            if (!context.BannerTexts.Any())
            {
                try
                {
                    context.BannerTexts.AddRange(
                        new BannerText
                        {
                            Language = Language.Fr,
                            Header = "Bienvenue a la TGV Ticket System",
                            Body = "Réservez vos places ici"
                        },
                        new BannerText
                        {
                            Language = Language.En,
                            Header = "Welcome to the TGV Ticket System",
                            Body = "Book your tickets here"
                        },
                        new BannerText
                        {
                            Language = Language.Ne,
                            Header = "Welkom bij de TGV Ticket System",
                            Body = "Reserveer uw tickets hier"
                        }
                    );

                    context.SaveChanges();
                }
                catch (FileNotFoundException exception)
                {
                    logger.LogError("BannerText file was not found : " + exception.FileName +
                                     ". BannerText table will be empty for this lang");
                }
            }

            AddCitySeedData(context, logger);
        }

        private static void AddCitySeedData(ApplicationDbContext context, ILogger _logger)
        {
            if (!context.Cities.Any())
            {
                context.Cities.AddRange(
                    new City {Name = "Paris"},
                    new City {Name = "Brussels"},
                    new City {Name = "London"},
                    new City {Name = "Amsterdam"},
                    new City {Name = "Berlin"}
                );
                context.SaveChanges();
            }
            AddTripSeedData(context, _logger);
        }

        private static Random random = new Random();

        private static void AddTripSeedData(ApplicationDbContext context, ILogger _logger)
        {
            foreach (var fromCity in context.Cities)
            {
                foreach (var toCity in context.Cities)
                {
                    if (toCity != fromCity)
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            DateTime date = DateTime.Today;

                            for (int j = 0; j < random.Next(1, 4); j++)
                            {
                                date = date.AddMinutes(random.Next(60, 1800));

                                context.Add(new Trip
                                {
                                    FreeBusinessPlaces = 50,
                                    FromCity = fromCity,
                                    ToCity = toCity,
                                    PricePerPerson = 20 + random.Next(10, 36),
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