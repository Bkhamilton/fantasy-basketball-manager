using FantasyBasketballApi.Models;

namespace FantasyBasketballApi.Services;

public interface INbaScheduleService
{
    List<NbaGame> GetTodaysGames();
}

public class NbaScheduleService : INbaScheduleService
{
    private readonly List<NbaGame> _games;

    public NbaScheduleService()
    {
        // Initialize with sample today's games
        var today = DateTime.Today;
        _games = new List<NbaGame>
        {
            new NbaGame
            {
                Id = 1,
                HomeTeam = "LAL",
                AwayTeam = "GSW",
                GameTime = today.AddHours(19).AddMinutes(30),
                Status = "Scheduled"
            },
            new NbaGame
            {
                Id = 2,
                HomeTeam = "PHI",
                AwayTeam = "BOS",
                GameTime = today.AddHours(19),
                Status = "Scheduled"
            },
            new NbaGame
            {
                Id = 3,
                HomeTeam = "PHX",
                AwayTeam = "DEN",
                GameTime = today.AddHours(21),
                Status = "Scheduled"
            },
            new NbaGame
            {
                Id = 4,
                HomeTeam = "SAS",
                AwayTeam = "DAL",
                GameTime = today.AddHours(20).AddMinutes(30),
                Status = "Scheduled"
            }
        };
    }

    public List<NbaGame> GetTodaysGames() => _games;
}
