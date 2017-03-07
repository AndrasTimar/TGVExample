using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models;
using AndrasTimarTGV.Models.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace AndrasTimarTGV.Models {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
        }
                 
        public DbSet<BannerText> BannerTexts { get; set; }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<City> Cities { get; set; }
    }
}
