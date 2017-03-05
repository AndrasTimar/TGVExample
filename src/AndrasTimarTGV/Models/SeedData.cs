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
            
            if (!context.Introductions.Any()) {
                try {
                    context.Introductions.AddRange(
                        new Introduction {
                            Name = "int-fr",
                            Language = Language.fr,
                            Content = System.IO.File.ReadAllText("int-fr.txt")
                        },
                        new Introduction {
                            Name = "int-en",
                            Language = Language.en,
                            Content = System.IO.File.ReadAllText("int-en.txt")
                        },
                        new Introduction {
                            Name = "int-ne",
                            Language = Language.ne,
                            Content = System.IO.File.ReadAllText("int-ne.txt")
                        }

                    );

                    context.SaveChanges();
                } catch (FileNotFoundException exception) {
                    _logger.LogError("Introduction file was not found : " + exception.FileName + ". Introduction table will be empty for this lang");
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
                            DateTime date = DateTime.Today;
                            for (int i = 0; i < 20; i++)
                            {
                                date = date.AddHours(random.Next(6, 22));
                                context.Add(new Trip
                                {
                                    FreePlaces = 300,
                                    FromCity = fromCity,
                                    ToCity = toCity,
                                    PricePerPerson = 24,
                                    Time = date.AddDays(i),
                                    TotalPlaces = 300,
                                });
                            }
                        }
                    }
                }
                context.SaveChanges();
            }
        }
    }
}
