namespace WorkWeekPlanner.Api.Features.Planner.Models;

public class WorkChunk()
{
    public string Id { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }
}