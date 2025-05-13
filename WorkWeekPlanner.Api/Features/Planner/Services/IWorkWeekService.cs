using WorkWeekPlanner.Api.Infrastructure.Repositories;

namespace WorkWeekPlanner.Api.Features.Planner.Services;

public interface IWorkWeekService
{
    Task SaveChunkInDayAsync(WorkChunk workChunk, int year, int weekNumber, string workDayId);
    Task SaveWorkWeekAsync(WorkWeek workWeek);
}

public class WorkWeekService(IWorkWeekRepository workWeekRepository) : IWorkWeekService
{
    private readonly IWorkWeekRepository _workWeekRepository = workWeekRepository;

    public async Task SaveChunkInDayAsync(WorkChunk workChunk, int year, int weekNumber, string workDayId)
    {
        // Retrieve the work week by ID
        var workWeek = await _workWeekRepository.ReadAsync(year, weekNumber);
        if (workWeek == null)
        {
            throw new ArgumentException("Work week not found.");
        }

        // Find the specific work day by ID
        var workDay = workWeek.Days.FirstOrDefault(d => d.Id == workDayId);
        if (workDay == null)
        {
            throw new ArgumentException("Work day not found.");
        }

        // Add or update the work chunk in the day's chunks
        var existingChunk = workDay.Chunks.FirstOrDefault(c => c?.Id == workChunk.Id);
        if (existingChunk != null)
        {
            // Update existing chunk
            existingChunk.Description = workChunk.Description;
            existingChunk.Start = workChunk.Start;
            existingChunk.End = workChunk.End;
        }
        else
        {
            // Add new chunk
            workDay.Chunks.Add(workChunk);
        }

        // Save the updated work week
        await _workWeekRepository.SaveAsync(workWeek);
    }

    public async Task SaveWorkWeekAsync(WorkWeek workWeek)
    {
        await _workWeekRepository.SaveAsync(workWeek);
    }
}