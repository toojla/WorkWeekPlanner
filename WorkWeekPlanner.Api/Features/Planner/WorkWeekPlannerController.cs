using WorkWeekPlanner.Api.Features.Planner.Services;

namespace WorkWeekPlanner.Api.Features.Planner;

[ApiController]
[Route("api/[controller]")]
public class WorkWeekPlannerController(IWorkWeekFactory workWeekFactory) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetWorkWeek()
    {
        var workWeek = await workWeekFactory.GetOrCreateAsync();
        return Ok(workWeek);
    }
}