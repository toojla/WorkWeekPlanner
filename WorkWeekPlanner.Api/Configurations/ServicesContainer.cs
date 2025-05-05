using Microsoft.Extensions.DependencyInjection.Extensions;
using WorkWeekPlanner.Api.Features.Login.Services;
using WorkWeekPlanner.Api.Features.Settings;
using WorkWeekPlanner.Shared;

namespace WorkWeekPlanner.Api.Configurations;

public static class ServicesContainer
{
    public static void AddServices(this IServiceCollection services, IAppSettings? appSettings)
    {
        ArgumentNullException.ThrowIfNull(appSettings);
        services.TryAddSingleton(appSettings);

        services.AddHttpClient();

        services.TryAddScoped<IAuthService, AuthService>();

        // Register LocalJsonRepository with a directory path
        services.AddSingleton<ILocalJsonRepository>(provider =>
        {
            var directoryPath = Path.Combine("E:\\Temp", "JsonData");
            return new LocalJsonRepository(directoryPath);
        });
    }
}