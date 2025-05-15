using WorkWeekPlanner.Api.Configurations;
using WorkWeekPlanner.Api.Features.Settings;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var appSettings = configuration.Get<AppSettings>();

// Add services to the container.
builder.Services.AddControllers();

// Fully qualify the method call to resolve ambiguity
OpenApiContainer.AddOpenApi(builder.Services);
builder.Services.AddServices(appSettings);
builder.Services.AddRepositories();
builder.Services.ConfigureAuthentication(appSettings);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();