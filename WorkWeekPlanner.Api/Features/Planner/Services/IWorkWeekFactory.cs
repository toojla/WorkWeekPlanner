namespace WorkWeekPlanner.Api.Features.Planner.Services;

public interface IWorkWeekFactory
{
    Task<WorkWeek> GetOrCreateAsync();

    Task<WorkWeek> GetOrCreateAsync(DateTime date);
}