using API.Extension;
using API.Util;

var builder = WebApplication.CreateBuilder(args);
builder.AddServices();

var app = builder.Build();
app.Configure();
await SeedDb.SeedUsers(app.Services.CreateScope().ServiceProvider);
app.Run();