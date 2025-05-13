using WorkWeekPlanner.Api.Infrastructure;
using WorkWeekPlanner.Api.Infrastructure.Repositories;

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
        var workWeek = CreateNewWorkWeek(firstDateOfWeek);
        await workWeekRepository.SaveAsync(workWeek);
        return workWeek;
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
        var workWeek = CreateNewWorkWeek(firstDateOfWeek);
        await workWeekRepository.SaveAsync(workWeek);
        return workWeek;
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