using WorkWeekPlanner.Api.Infrastructure.Repositories;

namespace WorkWeekPlanner.Api.Features.Planner.Services;

public class WorkWeekService(IWorkWeekRepository workWeekRepository) : IWorkWeekService
{
    public async Task SaveChunkInDayAsync(WorkChunk workChunk, int year, int weekNumber, string workDayId)
    {
        // Retrieve the work week by ID
        var workWeek = await workWeekRepository.ReadAsync(year, weekNumber);
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
        await workWeekRepository.SaveAsync(workWeek);
    }

    public async Task SaveWorkDayAsync(WorkDay workDay, int year, int weekNumber)
    {
        // Retrieve the work week by year and week number
        var workWeek = await workWeekRepository.ReadAsync(year, weekNumber);
        if (workWeek == null)
        {
            throw new ArgumentException("Work week not found.");
        }

        // Find the existing work day by ID
        var existingDay = workWeek.Days.FirstOrDefault(d => d.Id == workDay.Id);
        if (existingDay != null)
        {
            // Update properties
            existingDay.DayOfWeek = workDay.DayOfWeek;
            existingDay.Date = workDay.Date;
            existingDay.Chunks = workDay.Chunks;
        }
        else
        {
            // Add new work day
            workWeek.Days.Add(workDay);
        }

        // Save the updated work week
        await workWeekRepository.SaveAsync(workWeek);
    }

    public async Task SaveWorkWeekAsync(WorkWeek workWeek)
    {
        await workWeekRepository.SaveAsync(workWeek);
    }
}