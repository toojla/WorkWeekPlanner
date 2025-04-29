namespace WorkWeekPlanner.Api.Features.Login.Models;

public class AppUser(string id, string username, string password, string[] roles) : IAppUser
{
    public string Id { get; } = id;
    public string Username { get; } = username;
    public string Password { get; } = password;
    public string[] Roles { get; } = roles;
}