using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WorkWeekPlanner.Api.Features.Planner.Models;

public class WorkDay
{
    public string Id { get; set; } = string.Empty;
    public int DayOfWeek { get; set; } = 0;
    public DateTime Date { get; set; } = DateTime.Now;
    public List<WorkChunk?> Chunks { get; } = [];

    //[JsonConstructor]
    //public WorkDay()
    //{
    //}

    //public WorkDay(string workWeekId, DateTime date)
    //{
    //    Date = date;
    //    Id = $"{workWeekId}-{date:dddd-yyyyMMdd}"; // ex: "2024-W23-Monday-20240512"
    //    Chunks = [];
    //}

    //public WorkChunk AddChunk(TimeSpan start, TimeSpan end, string description)
    //{
    //    var chunk = new WorkChunk(this, start, end, description);
    //    Chunks.Add(chunk);
    //    return chunk;
    //}

    //public void RemoveChunk(string chunkId)
    //{
    //    Chunks.RemoveAll(c => c?.Id == chunkId);
    //}

    //public WorkChunk? GetChunk(string chunkId)
    //{
    //    return Chunks.FirstOrDefault(c => c?.Id == chunkId);
    //}
}