using Microsoft.AspNetCore.Mvc;
using FantasyBasketballApi.Models;
using FantasyBasketballApi.Services;

namespace FantasyBasketballApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayersController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpGet]
    public ActionResult<List<Player>> GetAll()
    {
        return Ok(_playerService.GetAllPlayers());
    }

    [HttpGet("{id}")]
    public ActionResult<Player> GetById(int id)
    {
        var player = _playerService.GetPlayerById(id);
        if (player == null)
            return NotFound();
        
        return Ok(player);
    }

    [HttpGet("with-games-today")]
    public ActionResult<List<Player>> GetWithGamesToday()
    {
        return Ok(_playerService.GetPlayersWithGamesToday());
    }
}
