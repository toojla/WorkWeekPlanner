using WorkWeekPlanner.Api.Features.Login.Models;

namespace WorkWeekPlanner.Api.Features.Login.Services;

public interface IUserRepository
{
    Task<IAppUser?> GetUserByIdAsync(string userName);
}