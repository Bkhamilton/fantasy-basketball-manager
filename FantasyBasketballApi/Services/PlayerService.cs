using FantasyBasketballApi.Models;

namespace FantasyBasketballApi.Services;

public interface IPlayerService
{
    List<Player> GetAllPlayers();
    Player? GetPlayerById(int id);
    List<Player> GetPlayersWithGamesToday();
    bool MovePlayerToStarting(int id);
    bool MovePlayerToBench(int id);
}

public class PlayerService : IPlayerService
{
    private readonly List<Player> _players;

    public PlayerService()
    {
        // Initialize with sample data
        _players = new List<Player>
        {
            new Player
            {
                Id = 1,
                Name = "LeBron James",
                Team = "LAL",
                Position = "SF",
                IsStarting = true,
                Stats = new PlayerStats
                {
                    Points = 25.4,
                    Rebounds = 7.2,
                    Assists = 7.8,
                    Steals = 1.2,
                    Blocks = 0.6,
                    FieldGoalPercentage = 52.3,
                    ThreePointPercentage = 38.1
                },
                Injury = new InjuryStatus { Status = "Healthy" },
                GameToday = new GameToday
                {
                    HasGame = true,
                    Opponent = "GSW",
                    Time = "7:30 PM",
                    IsHomeGame = true
                }
            },
            new Player
            {
                Id = 2,
                Name = "Stephen Curry",
                Team = "GSW",
                Position = "PG",
                IsStarting = true,
                Stats = new PlayerStats
                {
                    Points = 29.7,
                    Rebounds = 5.1,
                    Assists = 6.3,
                    Steals = 1.4,
                    Blocks = 0.3,
                    FieldGoalPercentage = 47.2,
                    ThreePointPercentage = 42.5
                },
                Injury = new InjuryStatus { Status = "Healthy" },
                GameToday = new GameToday
                {
                    HasGame = true,
                    Opponent = "LAL",
                    Time = "7:30 PM",
                    IsHomeGame = false
                }
            },
            new Player
            {
                Id = 3,
                Name = "Kevin Durant",
                Team = "PHX",
                Position = "SF",
                IsStarting = false,
                Stats = new PlayerStats
                {
                    Points = 27.3,
                    Rebounds = 6.8,
                    Assists = 5.2,
                    Steals = 0.9,
                    Blocks = 1.2,
                    FieldGoalPercentage = 53.1,
                    ThreePointPercentage = 39.7
                },
                Injury = new InjuryStatus { Status = "Questionable", Description = "Ankle soreness" },
                GameToday = new GameToday
                {
                    HasGame = true,
                    Opponent = "DEN",
                    Time = "9:00 PM",
                    IsHomeGame = true
                }
            },
            new Player
            {
                Id = 4,
                Name = "Giannis Antetokounmpo",
                Team = "MIL",
                Position = "PF",
                IsStarting = true,
                Stats = new PlayerStats
                {
                    Points = 31.2,
                    Rebounds = 11.8,
                    Assists = 5.7,
                    Steals = 1.1,
                    Blocks = 1.6,
                    FieldGoalPercentage = 58.7,
                    ThreePointPercentage = 27.3
                },
                Injury = new InjuryStatus { Status = "Healthy" },
                GameToday = new GameToday
                {
                    HasGame = false,
                    Opponent = null,
                    Time = null,
                    IsHomeGame = false
                }
            },
            new Player
            {
                Id = 5,
                Name = "Joel Embiid",
                Team = "PHI",
                Position = "C",
                IsStarting = false,
                Stats = new PlayerStats
                {
                    Points = 33.1,
                    Rebounds = 10.2,
                    Assists = 4.2,
                    Steals = 1.0,
                    Blocks = 1.7,
                    FieldGoalPercentage = 54.8,
                    ThreePointPercentage = 37.1
                },
                Injury = new InjuryStatus { Status = "Out", Description = "Knee injury" },
                GameToday = new GameToday
                {
                    HasGame = true,
                    Opponent = "BOS",
                    Time = "7:00 PM",
                    IsHomeGame = true
                }
            },
            new Player
            {
                Id = 6,
                Name = "Luka Doncic",
                Team = "DAL",
                Position = "PG",
                IsStarting = true,
                Stats = new PlayerStats
                {
                    Points = 32.8,
                    Rebounds = 8.4,
                    Assists = 8.9,
                    Steals = 1.4,
                    Blocks = 0.5,
                    FieldGoalPercentage = 48.7,
                    ThreePointPercentage = 36.2
                },
                Injury = new InjuryStatus { Status = "Healthy" },
                GameToday = new GameToday
                {
                    HasGame = true,
                    Opponent = "SAS",
                    Time = "8:30 PM",
                    IsHomeGame = false
                }
            },
            new Player
            {
                Id = 7,
                Name = "Damian Lillard",
                Team = "MIL",
                Position = "PG",
                IsStarting = false,
                Stats = new PlayerStats
                {
                    Points = 25.9,
                    Rebounds = 4.3,
                    Assists = 7.1,
                    Steals = 0.9,
                    Blocks = 0.3,
                    FieldGoalPercentage = 42.1,
                    ThreePointPercentage = 35.4
                },
                Injury = new InjuryStatus { Status = "Healthy" },
                GameToday = new GameToday
                {
                    HasGame = false,
                    Opponent = null,
                    Time = null,
                    IsHomeGame = false
                }
            },
            new Player
            {
                Id = 8,
                Name = "Jayson Tatum",
                Team = "BOS",
                Position = "SF",
                IsStarting = false,
                Stats = new PlayerStats
                {
                    Points = 26.9,
                    Rebounds = 8.1,
                    Assists = 4.9,
                    Steals = 1.0,
                    Blocks = 0.7,
                    FieldGoalPercentage = 46.6,
                    ThreePointPercentage = 37.6
                },
                Injury = new InjuryStatus { Status = "Healthy" },
                GameToday = new GameToday
                {
                    HasGame = true,
                    Opponent = "PHI",
                    Time = "7:00 PM",
                    IsHomeGame = false
                }
            }
        };
    }

    public List<Player> GetAllPlayers() => _players;

    public Player? GetPlayerById(int id) => _players.FirstOrDefault(p => p.Id == id);

    public List<Player> GetPlayersWithGamesToday() =>
        _players.Where(p => p.GameToday?.HasGame == true).ToList();

    public bool MovePlayerToStarting(int id)
    {
        var player = GetPlayerById(id);
        if (player == null) return false;
        
        player.IsStarting = true;
        return true;
    }

    public bool MovePlayerToBench(int id)
    {
        var player = GetPlayerById(id);
        if (player == null) return false;
        
        player.IsStarting = false;
        return true;
    }
}
