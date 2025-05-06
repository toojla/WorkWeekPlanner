namespace WorkWeekPlanner.Api.Features.Login.Services;

public interface IUserRepository
{
    Task<IAppUser?> GetUserByIdAsync(string userName);
}