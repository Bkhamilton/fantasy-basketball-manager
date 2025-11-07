using Microsoft.AspNetCore.Mvc;
using FantasyBasketballApi.Services;
using FantasyBasketballApi.Models;

namespace FantasyBasketballApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SportsDbController : ControllerBase
{
    private readonly ISportsDbService _sportsDbService;
    private readonly ILogger<SportsDbController> _logger;

    public SportsDbController(ISportsDbService sportsDbService, ILogger<SportsDbController> logger)
    {
        _sportsDbService = sportsDbService;
        _logger = logger;
    }

    /// <summary>
    /// Sanitizes a string for safe logging by removing control characters and limiting length
    /// </summary>
    private static string SanitizeForLogging(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;
        
        // Remove newlines, carriage returns, and other control characters that could be used for log injection
        var sanitized = new string(input.Where(c => !char.IsControl(c) && c < 127).ToArray());
        
        // Limit length to prevent log flooding
        return sanitized.Length > 200 ? sanitized.Substring(0, 200) : sanitized;
    }

    /// <summary>
    /// Fetches all NBA players from SportsDB API and inserts them into the database
    /// </summary>
    /// <returns>List of inserted players</returns>
    [HttpPost("fetch-and-insert-players")]
    public async Task<ActionResult<List<Player>>> FetchAndInsertPlayers()
    {
        try
        {
            _logger.LogInformation("Starting to fetch and insert NBA players");
            var players = await _sportsDbService.FetchAndInsertNbaPlayersAsync();
            _logger.LogInformation("Successfully fetched and inserted {Count} players", players.Count);
            
            return Ok(new
            {
                success = true,
                message = $"Successfully inserted {players.Count} players",
                players = players
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Configuration error while fetching players");
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching and inserting players");
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while fetching and inserting players",
                error = ex.Message
            });
        }
    }

    /// <summary>
    /// Fetches players for a specific NBA team from SportsDB API
    /// </summary>
    /// <param name="teamName">Team name in format City_TeamName (e.g., Los_Angeles_Lakers)</param>
    /// <returns>List of players for the specified team</returns>
    [HttpGet("fetch-players/{teamName}")]
    public async Task<ActionResult<List<SportsDbPlayer>>> FetchPlayersForTeam(string teamName)
    {
        // Validate team name format
        if (string.IsNullOrWhiteSpace(teamName))
        {
            return BadRequest(new
            {
                success = false,
                message = "Team name is required"
            });
        }

        if (!teamName.Contains('_'))
        {
            return BadRequest(new
            {
                success = false,
                message = "Team name must be in format City_TeamName (e.g., Los_Angeles_Lakers, Boston_Celtics)"
            });
        }

        try
        {
            _logger.LogInformation("Fetching players for team: {TeamName}", SanitizeForLogging(teamName));
            var players = await _sportsDbService.FetchPlayersForTeamAsync(teamName);
            
            return Ok(new
            {
                success = true,
                teamName = teamName,
                count = players.Count,
                players = players
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Configuration error while fetching players for team: {TeamName}", SanitizeForLogging(teamName));
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching players for team: {TeamName}", SanitizeForLogging(teamName));
            return StatusCode(500, new
            {
                success = false,
                message = $"An error occurred while fetching players for team {teamName}",
                error = ex.Message
            });
        }
    }
}
