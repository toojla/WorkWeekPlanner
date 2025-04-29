using Microsoft.Extensions.DependencyInjection.Extensions;
using WorkWeekPlanner.Api.Features.Login.Services;
using WorkWeekPlanner.Api.Features.Settings;

namespace WorkWeekPlanner.Api.Configurations;

public static class ServicesContainer
{
    public static void AddServices(this IServiceCollection services, IAppSettings? appSettings)
    {
        ArgumentNullException.ThrowIfNull(appSettings);
        services.TryAddSingleton(appSettings);

        services.AddHttpClient();

        services.TryAddScoped<IAuthService, AuthService>();
    }
}