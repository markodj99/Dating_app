using API.Extension;
using API.Middleware;
using API.Util;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppServices(builder.Configuration);
builder.Services.AddAuthServices(builder.Configuration);
builder.Services.AddSwgServices();

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await SeedDb.SeedUsers(app.Services.CreateScope().ServiceProvider);

app.Run();