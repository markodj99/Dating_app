using API.Data;
using API.Interface;
using API.Repository;
using API.Repository.IRepository;
using API.Service;
using Microsoft.EntityFrameworkCore;

namespace API.Extension
{
    public static class AppServiceExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();

            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
            );

            services.AddEndpointsApiExplorer();

            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IMemberRepository, MemberRepository>();

            return services;
        }
    }
}
