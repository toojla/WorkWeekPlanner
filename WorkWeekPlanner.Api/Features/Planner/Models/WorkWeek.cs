namespace WorkWeekPlanner.Api.Features.Planner.Models;

public class WorkWeek
{
    public string Id { get; set; } = string.Empty;
    public int Year { get; set; } = 0;
    public int WeekNumber { get; set; } = 0;
    public List<WorkDay> Days { get; set; } = [];

    public WorkDay GetDay(DayOfWeek dow) => Days.FirstOrDefault(d => d.Date.DayOfWeek == dow);
}