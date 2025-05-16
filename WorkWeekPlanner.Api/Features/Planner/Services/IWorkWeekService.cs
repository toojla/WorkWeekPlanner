namespace WorkWeekPlanner.Api.Features.Planner.Services;

public interface IWorkWeekService
{
    Task SaveChunkInDayAsync(WorkChunk workChunk, int year, int weekNumber, string workDayId);

    Task SaveWorkDayAsync(WorkDay workDay, int year, int weekNumber);

    Task SaveWorkWeekAsync(WorkWeek workWeek);
}