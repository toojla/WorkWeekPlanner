using WorkWeekPlanner.Api.Configurations;
using WorkWeekPlanner.Api.Features.Settings;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var appSettings = configuration.Get<AppSettings>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddServices(appSettings);
builder.Services.AddRepositories();
builder.Services.ConfigureAuthentication(appSettings);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
