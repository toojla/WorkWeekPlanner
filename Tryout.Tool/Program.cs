// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkWeekPlanner.Api.Features.Planner.Models;
using WorkWeekPlanner.Api.Features.Planner.Services;
using WorkWeekPlanner.Api.Infrastructure.Repositories;

Console.WriteLine("Trying out some stuff!");

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Register your services here
        services.AddSingleton<IWorkWeekFactory, WorkWeekFactory>();
        services.AddSingleton<IWorkWeekService, WorkWeekService>();
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
    var workWeekFactory = services.GetRequiredService<IWorkWeekFactory>();
    var workWeekService = services.GetRequiredService<IWorkWeekService>();
    var workWeek = workWeekFactory.GetOrCreateAsync(DateTime.Now).Result;
    Console.WriteLine($"Work Week ID: {workWeek.Id}");
    Console.WriteLine($"Year: {workWeek.Year}");
    Console.WriteLine($"Week Number: {workWeek.WeekNumber}");
    Console.WriteLine($"Days: {string.Join(", ", workWeek.Days.Select(d => d.Date.ToShortDateString()))}");
    var monday = workWeek.GetDay(DayOfWeek.Monday);
    var chunk = new WorkChunk
    {
        Id = Guid.NewGuid().ToString(),
        Start = new TimeSpan(9, 0, 0),
        End = new TimeSpan(17, 0, 0),
        Description = "Work on project X"
    };

    //workWeekService.SaveWorkWeekAsync(workWeek).Wait();
    workWeekService.SaveChunkInDayAsync(chunk, workWeek.Year, workWeek.WeekNumber, monday.Id).Wait();
}