using WorkWeekPlanner.Api.Features.Planner.Models;
using WorkWeekPlanner.Api.Infrastructure;
using WorkWeekPlanner.Api.Infrastructure.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WorkWeekPlanner.Api.Features.Planner.Services;

public class WorkWeekFactory(IWorkWeekRepository workWeekRepository) : IWorkWeekFactory
{
    public async Task<WorkWeek> GetOrCreateAsync()
    {
        var (year, weekNumber) = IsoWeekUtils.GetIso8601WeekOfYear(DateTime.Now);
        // Check if the WorkWeek exists on disk
        var existingWorkWeek = await workWeekRepository.ReadAsync(year, weekNumber);

        if (existingWorkWeek != null)
        {
            // Return the existing WorkWeek
            return existingWorkWeek;
        }

        // Create a new WorkWeek for the first date of the specified week
        var firstDateOfWeek = IsoWeekUtils.FirstDateOfWeekIso8601(year, weekNumber);
        return CreateNewWorkWeek(firstDateOfWeek);
    }

    public async Task<WorkWeek> GetOrCreateAsync(DateTime date)
    {
        var (year, weekNumber) = IsoWeekUtils.GetIso8601WeekOfYear(date);
        var existingWorkWeek = await workWeekRepository.ReadAsync(year, weekNumber);

        if (existingWorkWeek != null)
        {
            // Return the existing WorkWeek
            return existingWorkWeek;
        }

        // Create a new WorkWeek for the first date of the specified week
        var firstDateOfWeek = IsoWeekUtils.FirstDateOfWeekIso8601(year, weekNumber);
        return CreateNewWorkWeek(firstDateOfWeek);
    }

    private static WorkWeek CreateNewWorkWeek(DateTime forDate)
    {
        var (year, weekNumber) = IsoWeekUtils.GetIso8601WeekOfYear(forDate);

        var id = $"{year}-W{weekNumber}";
        var days = Enumerable.Range(0, 5) // Monday to Friday
            .Select(i =>
            {
                var dayDate = IsoWeekUtils.FirstDateOfWeekIso8601(year, weekNumber).AddDays(i);
                return new WorkDay
                {
                    Id = $"{id}-{forDate:dddd-yyyyMMdd}",
                    Date = dayDate
                };
            }).ToList();

        return new WorkWeek
        {
            Id = id,
            Year = year,
            WeekNumber = weekNumber,
            Days = days
        };
    }
}