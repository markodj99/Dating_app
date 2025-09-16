using API.Data;
using API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Util
{
    public class SeedDb
    {
        public static async Task SeedUsers(IServiceProvider services)
        {
            try
            {
                var context = services.GetRequiredService<AppDbContext>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                await context.Database.MigrateAsync();
                await context.Connections.ExecuteDeleteAsync();
                await Seed.SeedUsers(userManager);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during migration or data seeding.");
            }
        }
    }
}
