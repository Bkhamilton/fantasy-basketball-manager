using FantasyBasketballApi.Models;

namespace FantasyBasketballApi.Data;

public static class DbInitializer
{
    public static void Initialize(FantasyBasketballContext context)
    {
        // Ensure database is created
        context.Database.EnsureCreated();

        // Check if data already exists
        if (context.Players.Any())
        {
            return; // Database has been seeded
        }

        // Seed Players
        var players = new Player[]
        {
            new Player
            {
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
            },
            // Adding two more players
            new Player
            {
                Name = "Nikola Jokic",
                Team = "DEN",
                Position = "C",
                IsStarting = true,
                Stats = new PlayerStats
                {
                    Points = 26.4,
                    Rebounds = 12.4,
                    Assists = 9.0,
                    Steals = 1.3,
                    Blocks = 0.9,
                    FieldGoalPercentage = 63.2,
                    ThreePointPercentage = 35.9
                },
                Injury = new InjuryStatus { Status = "Healthy" },
                GameToday = new GameToday
                {
                    HasGame = true,
                    Opponent = "PHX",
                    Time = "9:00 PM",
                    IsHomeGame = false
                }
            },
            new Player
            {
                Name = "Anthony Davis",
                Team = "LAL",
                Position = "PF",
                IsStarting = true,
                Stats = new PlayerStats
                {
                    Points = 24.7,
                    Rebounds = 12.6,
                    Assists = 3.5,
                    Steals = 1.2,
                    Blocks = 2.3,
                    FieldGoalPercentage = 55.6,
                    ThreePointPercentage = 25.7
                },
                Injury = new InjuryStatus { Status = "Healthy" },
                GameToday = new GameToday
                {
                    HasGame = true,
                    Opponent = "GSW",
                    Time = "7:30 PM",
                    IsHomeGame = true
                }
            }
        };

        context.Players.AddRange(players);
        context.SaveChanges();

        // Seed NBA Games
        var games = new NbaGame[]
        {
            new NbaGame
            {
                HomeTeam = "LAL",
                AwayTeam = "GSW",
                GameTime = DateTime.Today.AddHours(19).AddMinutes(30),
                Status = "Scheduled"
            },
            new NbaGame
            {
                HomeTeam = "PHI",
                AwayTeam = "BOS",
                GameTime = DateTime.Today.AddHours(19),
                Status = "Scheduled"
            },
            new NbaGame
            {
                HomeTeam = "PHX",
                AwayTeam = "DEN",
                GameTime = DateTime.Today.AddHours(21),
                Status = "Scheduled"
            },
            new NbaGame
            {
                HomeTeam = "SAS",
                AwayTeam = "DAL",
                GameTime = DateTime.Today.AddHours(20).AddMinutes(30),
                Status = "Scheduled"
            }
        };

        context.NbaGames.AddRange(games);
        context.SaveChanges();
    }
}
