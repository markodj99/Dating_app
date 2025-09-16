namespace API.Extension
{
    public static class BuilderExtension
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddAppServices(builder.Configuration);
            builder.Services.AddAuthServices(builder.Configuration);
            builder.Services.AddSwgServices();
        }
    }
}
