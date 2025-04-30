namespace WorkWeekPlanner.Api.Features.Planner.Models;

public class WorkDay
{
    public WorkWeek Week { get; }
    public string Id { get; }
    public DayOfWeek DayOfWeek => Date.DayOfWeek;
    public DateTime Date { get; }
    public List<WorkChunk> Chunks { get; }

    public WorkDay(WorkWeek week, DateTime date)
    {
        Week = week;
        Date = date;
        Id = $"{week.Id}-{date:dddd-yyyyMMdd}"; // ex: "2024-W23-Monday-20240512"
        Chunks = [];
    }

    public WorkChunk AddChunk(TimeSpan start, TimeSpan end, string description)
    {
        var chunk = new WorkChunk(this, start, end, description);
        Chunks.Add(chunk);
        return chunk;
    }

    public void RemoveChunk(string chunkId)
    {
        Chunks.RemoveAll(c => c.Id == chunkId);
    }

    public WorkChunk GetChunk(string chunkId)
    {
        return Chunks.FirstOrDefault(c => c.Id == chunkId);
    }
}