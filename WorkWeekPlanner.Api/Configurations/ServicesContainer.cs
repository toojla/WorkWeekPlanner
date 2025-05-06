using Microsoft.Extensions.DependencyInjection.Extensions;
using WorkWeekPlanner.Api.Features.Login.Services;
using WorkWeekPlanner.Api.Features.Settings;
using WorkWeekPlanner.Api.Infrastructure.Repositories;

namespace WorkWeekPlanner.Api.Configurations;

public static class ServicesContainer
{
    public static void AddServices(this IServiceCollection services, IAppSettings? appSettings)
    {
        ArgumentNullException.ThrowIfNull(appSettings);
        services.TryAddSingleton(appSettings);

        services.AddHttpClient();

        services.TryAddScoped<IAuthService, AuthService>();

        services.AddSingleton<ILocalJsonRepository>(provider =>
        {
            var directoryPath = Path.Combine("E:\\Temp", "JsonData");
            return new LocalJsonRepository(directoryPath);
        });

        services.AddSingleton<IWorkWeekRepository>(provider =>
        {
            var directoryPath = Path.Combine("E:\\Temp", "WorkWeeks");
            return new WorkWeekRepository(directoryPath);
        });
    }
}