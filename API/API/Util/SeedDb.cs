using API.Data;
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
                await context.Database.MigrateAsync();
                await Seed.SeedUsers(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during migration or data seeding.");
            }
        }
    }
}
