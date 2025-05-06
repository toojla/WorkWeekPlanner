namespace WorkWeekPlanner.Api.Configurations;

public static class RepositoriesContainer
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }
}