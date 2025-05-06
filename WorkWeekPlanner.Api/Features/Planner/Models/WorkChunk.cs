namespace WorkWeekPlanner.Api.Features.Planner.Models;

public class WorkChunk
{
    public string Id { get; }
    public string Description { get; set; }
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }

    public WorkChunk(WorkDay day, TimeSpan start, TimeSpan end, string description)
    {
        Start = start;
        End = end;
        Description = description;

        Id = $"{day.Id}-C{start:hhmm}-{end:hhmm}";
    }
}