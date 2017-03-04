using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AndrasTimarTGV.Models
{
    public class IntroductionData
    {
        public static void AddIntroductionDataToDB(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            ILogger _logger = app.ApplicationServices.GetRequiredService<ILogger<IntroductionData>>();
            if (!context.Introductions.Any())
            {
                try
                {
                    context.Introductions.AddRange(
                        new Introduction
                        {
                            Name = "int-fr",
                            Language = Language.french,
                            Content = System.IO.File.ReadAllText("int-fr.txt")
                        },
                        new Introduction
                        {
                            Name = "int-en",
                            Language = Language.english,
                            Content = System.IO.File.ReadAllText("int-en.txt")
                        },
                        new Introduction
                        {
                            Name = "int-ne",
                            Language = Language.dutch,
                            Content = System.IO.File.ReadAllText("int-ne.txt")
                        }

                    );

                    context.SaveChanges();
                }
                catch (FileNotFoundException exception)
                {
                    _logger.LogError("Introduction file was not found : "+exception.FileName+". Introduction table will be empty for this lang");
                }
            }
        }
    }
}
