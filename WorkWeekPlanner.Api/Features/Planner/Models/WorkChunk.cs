namespace WorkWeekPlanner.Api.Features.Planner.Models;

public class WorkChunk()
{
    public string Id { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }
}

//public class WorkChunk(WorkDay day, TimeSpan start, TimeSpan end, string description)
//{
//    public string Id { get; } = $"{day.Id}-C{start:hhmm}-{end:hhmm}";
//    public string Description { get; set; } = description;
//    public TimeSpan Start { get; set; } = start;
//    public TimeSpan End { get; set; } = end;
//}