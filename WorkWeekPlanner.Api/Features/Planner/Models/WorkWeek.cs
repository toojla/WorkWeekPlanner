using System.Text.Json.Serialization;
using WorkWeekPlanner.Api.Infrastructure;

namespace WorkWeekPlanner.Api.Features.Planner.Models;

public class WorkWeek
{
    public string Id { get; set; } = string.Empty;
    public int Year { get; set; } = 0;
    public int WeekNumber { get; set; } = 0;
    public List<WorkDay> Days { get; set; } = [];

    //[JsonConstructor]
    //public WorkWeek()
    //{
    //}

    //public WorkWeek(DateTime forDate)
    //{
    //    (Year, WeekNumber) = IsoWeekUtils.GetIso8601WeekOfYear(forDate);

    //    Id = $"{Year}-W{WeekNumber}";
    //    Days = Enumerable.Range(0, 5) // Monday to Friday
    //        .Select(i =>
    //        {
    //            var dayDate = IsoWeekUtils.FirstDateOfWeekIso8601(Year, WeekNumber).AddDays(i);
    //            return new WorkDay(this.Id, dayDate);
    //        }).ToList();
    //}

    public WorkDay GetDay(DayOfWeek dow) => Days.FirstOrDefault(d => d.Date.DayOfWeek == dow);
}