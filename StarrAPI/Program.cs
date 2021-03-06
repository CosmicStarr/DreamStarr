using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StarrAPI.Data;
using StarrAPI.Models;

namespace StarrAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var scoope = host.Services.CreateScope();
            var Services = scoope.ServiceProvider;
            try
            {
                var DataContext = Services.GetRequiredService<ApplicationDbContext>();
                var userManager = Services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = Services.GetRequiredService<RoleManager<AppRole>>();
                await DataContext.Database.MigrateAsync();
                await Seed.SeedUsers(userManager,roleManager);
            }
            catch(Exception Ex)
            {
                var Logger = Services.GetRequiredService<ILogger<Program>>();
                Logger.LogError(Ex,"Something went wrong Normand!");
            }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
