using Microsoft.AspNetCore.Mvc;
using FantasyBasketballApi.Services;

namespace FantasyBasketballApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RosterController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public RosterController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpPost("move-to-starting/{id}")]
    public IActionResult MoveToStarting(int id)
    {
        var success = _playerService.MovePlayerToStarting(id);
        if (!success)
            return NotFound(new { message = "Player not found" });
        
        return Ok(new { message = "Player moved to starting lineup" });
    }

    [HttpPost("move-to-bench/{id}")]
    public IActionResult MoveToBench(int id)
    {
        var success = _playerService.MovePlayerToBench(id);
        if (!success)
            return NotFound(new { message = "Player not found" });
        
        return Ok(new { message = "Player moved to bench" });
    }

    [HttpGet]
    public IActionResult GetRoster()
    {
        var players = _playerService.GetAllPlayers();
        return Ok(new
        {
            starting = players.Where(p => p.IsStarting).ToList(),
            bench = players.Where(p => !p.IsStarting).ToList()
        });
    }
}
