using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StarrAPI.AutoMapperHelp;
using StarrAPI.Data;
using StarrAPI.Data.Interfaces;
using StarrAPI.Data.Repositories;
using StarrAPI.InterfacesandClasses;

namespace StarrAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoUpload,PhotoService>();
            services.AddScoped<ILikesRepository,LikeRepository>();
            services.AddScoped<LogUserActivity>();
            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddAutoMapper(typeof(AutoMapProfiles).Assembly);
            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}