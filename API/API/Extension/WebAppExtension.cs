using API.Middleware;
using API.SignalR;

namespace API.Extension
{
    public static class WebAppExtension
    {
        public static void Configure(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200", "https://localhost:4200"));

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.MapControllers();
            app.MapHub<PresenceHub>("hub/presence");
            app.MapHub<MessageHub>("hub/messages");

            app.MapFallbackToController("Index", "Fallback");
        }
    }
}
