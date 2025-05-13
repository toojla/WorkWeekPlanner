// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkWeekPlanner.Api.Features.Planner.Services;
using WorkWeekPlanner.Api.Infrastructure.Repositories;

Console.WriteLine("Trying out some stuff!");




var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Register your services here
        services.AddSingleton<IWorkWeekFactory, WorkWeekFactory>();
        services.AddSingleton<IWorkWeekRepository>(provider =>
        {
            var directoryPath = Path.Combine("E:\\Temp", "WorkWeeks");
            return new WorkWeekRepository(directoryPath);
        });
    }).Build();

var services = host.Services;

Tryout(services);

await host.RunAsync();

Console.WriteLine("Done!");

static void Tryout(IServiceProvider services)
{
    IWorkWeekFactory workWeekFactory = services.GetRequiredService<IWorkWeekFactory>();
    var workWeek = workWeekFactory.GetOrCreateAsync(DateTime.Now).Result;
    Console.WriteLine($"Work Week ID: {workWeek.Id}");
    Console.WriteLine($"Year: {workWeek.Year}");
    Console.WriteLine($"Week Number: {workWeek.WeekNumber}");
    Console.WriteLine($"Days: {string.Join(", ", workWeek.Days.Select(d => d.Date.ToShortDateString()))}");
}