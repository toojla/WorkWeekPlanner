namespace WorkWeekPlanner.Api.Features.Login.Models;

public interface IAppUser
{
    string Id { get; }
    string Username { get; }
    string Password { get; }
    string[] Roles { get; }
}