using WorkWeekPlanner.Api.Infrastructure;

namespace WorkWeekPlanner.Api.Features.Planner.Models;

public class WorkWeek
{
    public string Id { get; }
    public int Year { get; }
    public int WeekNumber { get; }
    public List<WorkDay> Days { get; }

    public WorkWeek(DateTime forDate)
    {
        (Year, WeekNumber) = IsoWeekUtils.GetIso8601WeekOfYear(forDate);

        Id = $"{Year}-W{WeekNumber}";
        Days = Enumerable.Range(0, 5) // Monday to Friday
            .Select(i =>
            {
                var dayDate = IsoWeekUtils.FirstDateOfWeekIso8601(Year, WeekNumber).AddDays(i);
                return new WorkDay(this, dayDate);
            }).ToList();
    }

    public WorkDay GetDay(DayOfWeek dow) => Days.FirstOrDefault(d => d.Date.DayOfWeek == dow);
}