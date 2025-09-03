using API.Extension;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppServices(builder.Configuration);
builder.Services.AddAuthServices(builder.Configuration);
builder.Services.AddSwgServices();

var app = builder.Build();

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

app.Run();