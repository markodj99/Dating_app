using API.Data;
using API.Interface;
using API.Repository;
using API.Repository.IRepository;
using API.Service;
using API.SignalR;
using API.Util;
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
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IHubService, HubService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<ILikesRepository, LikesRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<LogUserActivity>();

            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            services.AddSignalR();
            services.AddSingleton<PresenceTracker>();

            return services;
        }
    }
}
