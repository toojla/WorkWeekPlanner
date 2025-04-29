namespace WorkWeekPlanner.Api.Features.Settings;

public class AppSettings : IAppSettings
{
    public AppConfiguration AppConfiguration { get; set; } = new();
}