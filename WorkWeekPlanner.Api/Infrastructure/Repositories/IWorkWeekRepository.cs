namespace WorkWeekPlanner.Api.Infrastructure.Repositories;

public interface IWorkWeekRepository
{
    void Delete(int year, int weekNumber);

    IEnumerable<string> ListAllWorkWeeks();

    Task<WorkWeek?> ReadAsync(int year, int weekNumber);

    Task SaveAsync(WorkWeek workWeek);
}