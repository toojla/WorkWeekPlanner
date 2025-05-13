namespace WorkWeekPlanner.Api.Features.Planner.Models;

public class WorkDay
{
    public string Id { get; set; } = string.Empty;
    public int DayOfWeek { get; set; } = 0;
    public DateTime Date { get; set; } = DateTime.Now;
    public List<WorkChunk?> Chunks { get; set; } = [];
}