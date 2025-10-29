using Microsoft.AspNetCore.Mvc;
using FantasyBasketballApi.Models;
using FantasyBasketballApi.Services;

namespace FantasyBasketballApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly INbaScheduleService _scheduleService;

    public ScheduleController(INbaScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpGet("today")]
    public ActionResult<List<NbaGame>> GetTodaysGames()
    {
        return Ok(_scheduleService.GetTodaysGames());
    }
}
