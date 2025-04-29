using WorkWeekPlanner.Api.Features.Login.Models;

namespace WorkWeekPlanner.Api.Features.Login.Services;

public class UserRepository : IUserRepository
{
    public Task<IAppUser?> GetUserByIdAsync(string userName)
    {
        return Task.FromResult(GetUser(userName));
    }

    private IAppUser? GetUser(string userName)
    {
        // Simulate fetch from database
        var userId = $"3B9D6C39-6D76-4D92-B4D6-14899A3C58C7:{userName}";
        return new AppUser(userId, userName, "password", ["admin", "user"]);
    }
}