namespace WorkWeekPlanner.Api.Infrastructure.Repositories;

public interface IWorkWeekRepository
{
    Task SaveAsync(WorkWeek workWeek);

    Task<WorkWeek?> ReadAsync(int year, int weekNumber);

    void Delete(int year, int weekNumber);

    IEnumerable<string> ListAllWorkWeeks();
}