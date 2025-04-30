namespace WorkWeekPlanner.Api.Features.Settings;

public class AppConfiguration
{
    public string Audience { get; set; } = string.Empty;
    public int ExpiryInHours { get; set; }
    public string Issuer { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
}