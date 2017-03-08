using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AndrasTimarTGV.Models;
using AndrasTimarTGV.Models.Entities;
using AndrasTimarTGV.Models.Repositories;
using AndrasTimarTGV.Models.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AndrasTimarTGV
{
    public class Startup
    {
        private IConfigurationRoot Configuration;
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940

        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json")
               .Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                Configuration["Data:TGVIdentity:ConnectionString"]));            

            services.AddIdentity<AppUser, IdentityRole>(options => {
                    options.Cookies.ApplicationCookie.LoginPath = "/Account/Login";
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;                    
                }
            ).AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddTransient<IBannerTextRepository, EFBannerTextRepository>();
            services.AddTransient<IBannerTextService, BannerTextService>();
            services.AddTransient<ITripRepository, EFTripRepository>();
            services.AddTransient<ITripService, TripService>();
            services.AddTransient<ICityRepository, EFCityRepository>();
            services.AddTransient<ICityService, CityService>();
            services.AddTransient<IReservationRepository, EFReservationRepository>();
            services.AddTransient<IReservationService, ReservationService>();
            services.AddMvc();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseSession();
            app.UseIdentity();
            app.UseMvc(routes =>
            {
                
                routes.MapRoute(
                    name: null,
                    template: "{controller}/{action}",
                    defaults: new {controller = "Home", action = "Index"});

                routes.MapRoute(
                    name: null,
                    template: "",
                    defaults: new {controller = "Home", action = "Index"});
                routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
            });
            SeedData.AddSeedData(app);
        }
    }
}
