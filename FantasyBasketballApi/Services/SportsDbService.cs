using System.Net.Http;
using System.Text.Json;
using FantasyBasketballApi.Models;
using FantasyBasketballApi.Data;
using Microsoft.EntityFrameworkCore;

namespace FantasyBasketballApi.Services;

public interface ISportsDbService
{
    Task<List<Player>> FetchAndInsertNbaPlayersAsync();
    Task<List<SportsDbPlayer>> FetchPlayersForTeamAsync(string teamName);
}

public class SportsDbService : ISportsDbService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly FantasyBasketballContext _context;
    private readonly ILogger<SportsDbService> _logger;

    // NBA teams in the format required by SportsDB API
    private static readonly string[] NbaTeams = 
    {
        "Atlanta_Hawks",
        "Boston_Celtics",
        "Brooklyn_Nets",
        "Charlotte_Hornets",
        "Chicago_Bulls",
        "Cleveland_Cavaliers",
        "Dallas_Mavericks",
        "Denver_Nuggets",
        "Detroit_Pistons",
        "Golden_State_Warriors",
        "Houston_Rockets",
        "Indiana_Pacers",
        "Los_Angeles_Clippers",
        "Los_Angeles_Lakers",
        "Memphis_Grizzlies",
        "Miami_Heat",
        "Milwaukee_Bucks",
        "Minnesota_Timberwolves",
        "New_Orleans_Pelicans",
        "New_York_Knicks",
        "Oklahoma_City_Thunder",
        "Orlando_Magic",
        "Philadelphia_76ers",
        "Phoenix_Suns",
        "Portland_Trail_Blazers",
        "Sacramento_Kings",
        "San_Antonio_Spurs",
        "Toronto_Raptors",
        "Utah_Jazz",
        "Washington_Wizards"
    };

    // Team abbreviation mapping
    private static readonly Dictionary<string, string> TeamAbbreviations = new()
    {
        { "Atlanta_Hawks", "ATL" },
        { "Boston_Celtics", "BOS" },
        { "Brooklyn_Nets", "BKN" },
        { "Charlotte_Hornets", "CHA" },
        { "Chicago_Bulls", "CHI" },
        { "Cleveland_Cavaliers", "CLE" },
        { "Dallas_Mavericks", "DAL" },
        { "Denver_Nuggets", "DEN" },
        { "Detroit_Pistons", "DET" },
        { "Golden_State_Warriors", "GSW" },
        { "Houston_Rockets", "HOU" },
        { "Indiana_Pacers", "IND" },
        { "Los_Angeles_Clippers", "LAC" },
        { "Los_Angeles_Lakers", "LAL" },
        { "Memphis_Grizzlies", "MEM" },
        { "Miami_Heat", "MIA" },
        { "Milwaukee_Bucks", "MIL" },
        { "Minnesota_Timberwolves", "MIN" },
        { "New_Orleans_Pelicans", "NOP" },
        { "New_York_Knicks", "NYK" },
        { "Oklahoma_City_Thunder", "OKC" },
        { "Orlando_Magic", "ORL" },
        { "Philadelphia_76ers", "PHI" },
        { "Phoenix_Suns", "PHX" },
        { "Portland_Trail_Blazers", "POR" },
        { "Sacramento_Kings", "SAC" },
        { "San_Antonio_Spurs", "SAS" },
        { "Toronto_Raptors", "TOR" },
        { "Utah_Jazz", "UTA" },
        { "Washington_Wizards", "WAS" }
    };

    public SportsDbService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        FantasyBasketballContext context,
        ILogger<SportsDbService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _context = context;
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

    public async Task<List<SportsDbPlayer>> FetchPlayersForTeamAsync(string teamName)
    {
        var apiKey = _configuration["SPORTSDB_API_KEY"];
        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogError("SPORTSDB_API_KEY is not configured");
            throw new InvalidOperationException("SPORTSDB_API_KEY is not configured in environment variables or appsettings.json");
        }

        var url = $"https://www.thesportsdb.com/api/v1/json/{apiKey}/searchplayers.php?t={teamName}";
        _logger.LogInformation("Fetching players for team: {TeamName}", SanitizeForLogging(teamName));

        var client = _httpClientFactory.CreateClient();
        
        try
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SportsDbPlayerResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Player ?? new List<SportsDbPlayer>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while fetching players for team: {TeamName}", SanitizeForLogging(teamName));
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error for team: {TeamName}", SanitizeForLogging(teamName));
            throw;
        }
    }

    public async Task<List<Player>> FetchAndInsertNbaPlayersAsync()
    {
        _logger.LogInformation("Starting to fetch and insert NBA players from SportsDB");
        var insertedPlayers = new List<Player>();
        var totalPlayers = 0;

        foreach (var teamName in NbaTeams)
        {
            try
            {
                _logger.LogInformation("Processing team: {TeamName}", teamName);
                var sportsDbPlayers = await FetchPlayersForTeamAsync(teamName);
                
                if (sportsDbPlayers.Count == 0)
                {
                    _logger.LogWarning("No players found for team: {TeamName}", teamName);
                    continue;
                }

                totalPlayers += sportsDbPlayers.Count;

                // Get team abbreviation
                if (!TeamAbbreviations.TryGetValue(teamName, out var teamAbbr))
                {
                    _logger.LogWarning("Team {TeamName} not found in abbreviation mapping, using fallback", teamName);
                    teamAbbr = teamName.Replace("_", "").Substring(0, Math.Min(3, teamName.Replace("_", "").Length)).ToUpper();
                }

                // Load all existing players for this team upfront to avoid N+1 queries
                var existingPlayers = await _context.Players
                    .Where(p => p.Team == teamAbbr)
                    .Select(p => p.Name)
                    .ToListAsync();
                
                var existingPlayerNames = new HashSet<string>(existingPlayers, StringComparer.OrdinalIgnoreCase);

                foreach (var sportsDbPlayer in sportsDbPlayers)
                {
                    // Check if player already exists using the in-memory set
                    if (existingPlayerNames.Contains(sportsDbPlayer.StrPlayer ?? ""))
                    {
                        _logger.LogInformation("Player {PlayerName} already exists, skipping", sportsDbPlayer.StrPlayer);
                        continue;
                    }

                    // Create new player
                    var player = new Player
                    {
                        Name = sportsDbPlayer.StrPlayer ?? "Unknown",
                        Team = teamAbbr,
                        Position = MapPosition(sportsDbPlayer.StrPosition),
                        IsStarting = false,
                        Stats = new PlayerStats
                        {
                            Points = 0,
                            Rebounds = 0,
                            Assists = 0,
                            Steals = 0,
                            Blocks = 0,
                            FieldGoalPercentage = 0,
                            ThreePointPercentage = 0
                        },
                        Injury = new InjuryStatus
                        {
                            Status = "Healthy"
                        },
                        GameToday = new GameToday
                        {
                            HasGame = false,
                            IsHomeGame = false
                        }
                    };

                    _context.Players.Add(player);
                    insertedPlayers.Add(player);
                    _logger.LogInformation("Added player: {PlayerName} - {Team} - {Position}", 
                        player.Name, player.Team, player.Position);
                }

                // Add a small delay to avoid overwhelming the API
                await Task.Delay(250);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing team: {TeamName}", teamName);
                // Continue with next team instead of stopping
            }
        }

        // Save all changes at once after processing all teams
        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Completed fetching players. Total fetched: {TotalPlayers}, Inserted: {InsertedCount}", 
                totalPlayers, insertedPlayers.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving players to database");
            throw;
        }

        return insertedPlayers;
    }

    private string MapPosition(string? position)
    {
        if (string.IsNullOrEmpty(position))
            return "Unknown";

        // Map common position variations to standard positions
        return position.ToUpper() switch
        {
            "POINT GUARD" or "PG" => "PG",
            "SHOOTING GUARD" or "SG" => "SG",
            "SMALL FORWARD" or "SF" => "SF",
            "POWER FORWARD" or "PF" => "PF",
            "CENTER" or "C" => "C",
            "GUARD" or "G" => "G",
            "FORWARD" or "F" => "F",
            _ => position
        };
    }
}
